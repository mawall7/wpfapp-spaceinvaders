using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace spaceinvaders
//{
    
    public class Timer
    {
        public DateTime StartTime { get; set; }
        public int EndTimeMilliSeconds { get; set; }
        
        public void StartTimer (int MilliSecondsTotime)
        {
            StartTime = DateTime.Now;
            this.EndTimeMilliSeconds = MilliSecondsTotime;
            
        }

        public bool TimeIsUp()
        {
            return (DateTime.Now.Subtract(StartTime).Milliseconds >= this.EndTimeMilliSeconds);
        }
    }
//}
