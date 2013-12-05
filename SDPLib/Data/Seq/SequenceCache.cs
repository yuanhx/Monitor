using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Services;

namespace SDP.Data.Seq
{
    public interface ISequenceCache
    {
        String SEQName { get; }
        int Step { get; }
	
	    long GetCurValue();
	    long GetNextValue();	
	    void Refresh();
    }

    public class SequenceCache : ISequenceCache
    {
        private String mSeqName = "";
        private long mValue = 0;
        private int mIncValue = 1;
        private int mStep = 1;
	
	    public SequenceCache(String seqname, int step) 
        {
		    mSeqName = seqname;
		    if (step>1)
			    mStep = step;
		    else mStep = 1;
		
		    mIncValue = mStep;
	    }
	
	    public String SEQName
        {
		    get { return mSeqName; }
	    }
	
	    public int Step 
        {
		    get { return mStep; }
	    }
	
	    public long GetCurValue()
	    {		
            lock(this)
            {
		        if (mIncValue < mStep)
		        {
			        return mValue + mIncValue;
		        }
		        else return RefreshCurValue();
            }
	    }
	
	    public long GetNextValue()
	    {
            lock(this)
            {
		        if (mIncValue < mStep)
		        {
			        return mValue + mIncValue++;
		        }
		        else return RefreshNextValue();
            }
	    }
	
	    private long RefreshCurValue()
	    {
		    mValue = DataExServices.GetSEQNextVal(mSeqName);
		    mIncValue = 0;
		
		    return mValue + mIncValue;
	    }
	
	    private long RefreshNextValue()
	    {
            mValue = DataExServices.GetSEQNextVal(mSeqName);
		    mIncValue = 0;
		
		    return mValue + mIncValue++;
	    }

	    public void Refresh()
        {
            lock (this)
            {
                RefreshCurValue();
            }
	    }
    }
}
