using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ImageSDK
{
    /// <summary>
    /// 抽象图像接口
    /// </summary>
	public interface IImage:IDisposable
	{
        /// <summary>
        /// 绘制
        /// </summary>e
        void Draw(IntPtr handle, int width, int height);

        void Draw(IntPtr handle, Rectangle destRect, int width, int height);

        void Draw(Graphics g ,int left, int top, int width, int height);
        /// <summary>
        /// 保存到文件
        /// </summary>
        void SaveToFile(string fileaName);

        void SaveToStream(System.IO.Stream sm);

        void fromImage(IImage src);

        IntPtr getHBitmap();

        long addRef();
        long releaseRef();
    }
}
