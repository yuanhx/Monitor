using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Services;

namespace SDP.Data.Seq
{
    public class SEQManager
    {
        private Dictionary<string, ISequenceCache> m_sequences = new Dictionary<string, ISequenceCache>();

        public ISequenceCache FindSequenceCache(string seqname)
        {
            if (m_sequences.ContainsKey(seqname))
                return m_sequences[seqname];
            else
                return null;
        }

        public ISequenceCache GetSequenceCache(string seqname, int step)
        {
            ISequenceCache seqCache = null;

            if (m_sequences.ContainsKey(seqname))
                seqCache = m_sequences[seqname];

            if (seqCache == null)
            {
                lock (m_sequences)
                {
                    if (m_sequences.ContainsKey(seqname))
                        seqCache = m_sequences[seqname];

                    if (seqCache == null)
                    {
                        seqCache = new SequenceCache(seqname, step);
                        m_sequences.Add(seqname, seqCache);
                    }
                }
            }

            return seqCache;
        }

        public long GetSEQNextValue(String seqname, int step)
        {
            ISequenceCache seqCache = GetSequenceCache(seqname, step);
            if (seqCache != null)
                return seqCache.GetNextValue();
            else
                return -1;
        }

        public long GetSEQNextValue(string seqname)
        {
            return GetSEQNextValue(seqname, 1);
        }

        public void ClearSEQValue()
        {
            lock (m_sequences)
		    {
			    m_sequences.Clear();
		    }
        }
    }
}
