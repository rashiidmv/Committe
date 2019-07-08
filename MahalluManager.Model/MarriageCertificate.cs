using System;

namespace MahalluManager.Model {
    public class MarriageCertificate {
        public int Id { get; set; }
        public String BrideName { get; set; }
        public byte[] BridePhoto { get; set; }
        public DateTime BrideDOB { get; set; }
        public string BrideFatherName { get; set; }
        public String BrideHouseName { get; set; }
        public String BrideArea { get; set; }
        public int BridePinCode { get; set; }
        public String BridePostOffice { get; set; }
        public String BrideDistrict { get; set; }
        public int BrideState { get; set; }

        public String GroomName { get; set; }
        public byte[] GroomPhoto { get; set; }
        public DateTime GroomDOB { get; set; }
        public string GroomFatherName { get; set; }
        public String GroomHouseName { get; set; }
        public String GroomArea { get; set; }
        public int GroomPincode { get; set; }
        public String GroomPostOffice { get; set; }
        public String GroomDistrict { get; set; }
        public int GroomState { get; set; }
    }
}
