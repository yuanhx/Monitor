using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace loglib.Log.Impl
{
    public class CTxtFileLog : CLog
    {
        private FileStream mFileStream = null;
        private StreamWriter mStreamWriter = null;

        public override long GetSize()
        {
            return mFileStream != null ? mFileStream.Position : 0;
        }

        protected override bool PrepInit()
        {
            if (mStreamWriter == null)
            {
                if (mFileStream == null)
                {
                    string path = this.Location;
                    if (path == null || path.Equals(""))
                    {
                        path = string.Format("{0}\\log\\log.txt", Path.GetDirectoryName(Application.ExecutablePath));
                        Location = path;
                    }

                    if (CheckFile(path))
                    {
                        mFileStream = new FileStream(path, FileMode.Append, FileAccess.Write);
                    }
                }

                if (mFileStream != null)
                {
                    mStreamWriter = new StreamWriter(mFileStream);
                    mStreamWriter.AutoFlush = true;
                    return base.PrepInit();
                }
            }
            return false;
        }

        protected override bool PrepCleanup()
        {
            if (mFileStream != null)
            {
                mFileStream.Close();
                mFileStream = null;
            }

            if (mStreamWriter != null)
            {
                mStreamWriter = null;
            }            

            return base.PrepCleanup();
        }

        protected override void WriteLog(string msg)
        {
            if (mStreamWriter != null)
            {
                mStreamWriter.WriteLine(msg);
            }
        }
    }
}
