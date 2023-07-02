using System;
using System.Collections.Generic;
using System.Drawing;
using static StbRectPackSharp.StbRectPack;

namespace StbRectPackSharp
{
#if !STBSHARP_INTERNAL
	public
#else
	internal
#endif
	class PackerRectangle
	{
		public Rectangle Rectangle { get; private set; }

		public int X => Rectangle.X;
		public int Y => Rectangle.Y;
		public int Width => Rectangle.Width;
		public int Height => Rectangle.Height;

		public object Data { get; private set; }

		public PackerRectangle(Rectangle rect, object data)
		{
			Rectangle = rect;
			Data = data;
		}
	}

	/// <summary>
	/// Simple Packer class that doubles size of the atlas if the place runs out
	/// </summary>
#if !STBSHARP_INTERNAL
	public
#else
	internal
#endif
	unsafe class Packer : IDisposable
	{
		private readonly stbrp_context _context;
		private readonly List<PackerRectangle> _rectangles = new List<PackerRectangle>();

		public int Width => _context.width;
		public int Height => _context.height;

		public List<PackerRectangle> PackRectangles => _rectangles;


		public Packer(int width = 256, int height = 256)
		{
			if (width <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(width));
			}

			if (height <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(height));
			}

			// Initialize the context
			var num_nodes = width;
			_context = new stbrp_context(num_nodes);

			fixed (stbrp_context* contextPtr = &_context)
			{
				stbrp_init_target(contextPtr, width, height, _context.all_nodes, num_nodes);
			}
		}

		public void Dispose()
		{
			_context.Dispose();
		}

		/// <summary>
		/// Packs a rect. Returns null, if there's no more place left.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="userData"></param>
		/// <returns></returns>
		public PackerRectangle PackRect(int width, int height, object userData)
		{
			var rect = new stbrp_rect
			{
				id = _rectangles.Count,
				w = width,
				h = height
			};

			int result;
			fixed (stbrp_context* contextPtr = &_context)
			{
				result = stbrp_pack_rects(contextPtr, &rect, 1);
			}

			if (result == 0)
			{
				return null;
			}

			var packRectangle = new PackerRectangle(new Rectangle(rect.x, rect.y, rect.w, rect.h), userData);
			_rectangles.Add(packRectangle);

			return packRectangle;
		}

		/// <summary>
		/// Packs a rect. Resizes the entire packer if there's no more space
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="userData"></param>
		/// <returns></returns>
		public static PackerRectangle PackRectForce(ref Packer packer, int width, int height, object userData)
		{
			PackerRectangle pr = packer.PackRect(width, height, userData);

			// If pr is null, it means there's no place for the new rect
			// Double the size of the packer until the new rectangle will fit
			while (pr == null)
			{
				Packer newPacker = new Packer(packer.Width + 128, packer.Height + 128);

				// Place existing rectangles
				foreach (PackerRectangle existingRect in packer.PackRectangles)
				{
					newPacker.PackRect(existingRect.Width, existingRect.Height, existingRect.Data);
				}

				// Now dispose old packer and assign new one
				packer.Dispose();
				packer = newPacker;

				// Try to fit the rectangle again
				pr = packer.PackRect(width, height, userData);
			}

			return pr;
		}

	}
}
