﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCADA
{
    public class LayerData
    {
        public IList<Button> Buttons
        {
            get;
            private set;
        }

        public LayerData(params Button[] buttons)
        {
            Buttons = buttons.ToList();
        }

        public bool IsThis(IList<Button> buttons)
        {
            foreach (var item in buttons)
            {
                if (!Buttons.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Buttons)
            {
                sb.Append(item.Text + " ");
            }
            return sb.ToString();
        }
    }
}
