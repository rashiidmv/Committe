using MahalluManager.DataAccess;
using MahalluManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataLoader {
    class Program {
        static void Main(string[] args) {
            try {

                Console.WriteLine("Enter Soure file path");
                String path = Console.ReadLine(); ;
                while(!File.Exists(path)) {
                    Console.WriteLine("Please enter correct source file path");
                    path = Console.ReadLine(); ;
                    Console.Read();
                }

                string[] inputlines = File.ReadAllLines(path);
                String currentFolder = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                String skippedMembers = Path.Combine(currentFolder, "skipped.csv");
                String skippedContent = inputlines[0];
                using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {

                    List<Residence> residences = unitOfWork.Residences.GetAll().ToList();
                    for(int i = 1; i < inputlines.Length; i++) {
                        string[] fields = inputlines[i].Split(',');
                        Residence residence = null;
                        bool isFound = false;
                        foreach(var item in residences) {
                            if(item.Number == fields[1].Trim('\"')) {
                                residence = item;
                                isFound = true;
                                break;
                            }
                        }
                        if(isFound) {
                            ResidenceMember residenceMember = GetResidenceMember(fields, residence.Id);
                            unitOfWork.ResidenceMembers.Add(residenceMember);
                            unitOfWork.Complete();
                        } else {
                            skippedContent += "\n" + inputlines[i];
                        }
                    }
                }
                File.WriteAllText(skippedMembers, skippedContent); ;

                Console.WriteLine("Data loaded successfully");
            } catch(Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            Console.Read();
        }
        private static ResidenceMember GetResidenceMember(string[] fields, int residenceId) {
            var residenceMember = new ResidenceMember();
            residenceMember.Residence_Id = residenceId;
            residenceMember.MemberName = fields[2].Trim('\"');
            residenceMember.DOB = Convert.ToDateTime(fields[3].Trim('\"'));
            residenceMember.Gender = fields[4].Trim('\"');
            residenceMember.Mobile = fields[5].Trim('\"');
            residenceMember.Job = fields[6].Trim('\"');
            residenceMember.MarriageStatus = fields[7].Trim('\"');
            residenceMember.Qualification = fields[8].Trim('\"');
            if(!String.IsNullOrEmpty(fields[9].Trim('\"'))) {
                residenceMember.Abroad = true;
                residenceMember.Country = fields[9].Trim('\"');
            } else {
                residenceMember.Abroad = false;
            }
            residenceMember.IsGuardian = false;

            return residenceMember;
        }
    }
}
