﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCADA
{
    public class ROBOT
    {
        public Int32 serial { get; set; }
        public String id;
        public String workshop { get; set; }
        public String productionline { get; set; }
        public String type { get; set; }
        public String system { get; set; }
        public String ip { get; set; }
        public ushort port { get; set; }
        public String EQUIP_CODE;
        public Int32 PLCAdressStar_X { get; set; }
        public Int32 AdressSum_X;
        public Int32 PLCAdressStar_Y { get; set; }
        public Int32 AdressSum_Y;
        public String SN;
        public void SetRobot(String serial, String id, String workshop, String productionline,
            String type, String system, String ip, String port, String EQUIP_CODE,String SN,
            Int32 PLCAdressStar_X, Int32 AdressSum_X, Int32 PLCAdressStar_Y, Int32 AdressSum_Y)
        {
            this.serial = Int32.Parse(serial);
            this.id = id;
            this.workshop = workshop;       
            this.productionline = productionline;
            this.type = type;
            this.system = system;
            this.ip = ip;
            this.port = ushort.Parse(port);
            this.EQUIP_CODE = EQUIP_CODE;
            this.SN = SN;
            this.PLCAdressStar_X = PLCAdressStar_X;
            this.PLCAdressStar_Y = PLCAdressStar_Y;
            this.AdressSum_X = AdressSum_X;
            this.AdressSum_Y = AdressSum_Y;
        }
    }
}
