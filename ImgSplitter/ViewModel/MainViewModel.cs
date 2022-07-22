using System;
using System.Collections.Generic;
using System.Text;

namespace ImgSplitter.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {    
            MeasuresValue = 2;
            StartNumberValue = 1;
        }

        public int MeasuresValue { get; set; }
        public int StartNumberValue { get; set; }
    }
}
