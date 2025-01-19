using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace MyDVLD.Global_Classes
{
    public  class clsUtil
    {
        public static string GenerateGuid()
        {
            Guid guid = new Guid();
            return guid.ToString();
        }

        public static bool CreateFolderIfDoesNotExist(string FolderPath)
        {
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch(IOException ex)
                {
                    MessageBox.Show("Error While bCreating Folder : "+ex.Message);
                    return false;
                }
            }
            return true;
        }

        public  static  string ReplaceFileNameByGuid(string FileName)
        {
            FileInfo fileInfo = new FileInfo(FileName);
            string Ext = fileInfo.Extension;
            return GenerateGuid()+Ext;
        }

        public static bool CopyImageToImageFolder(ref string SourceFile)
        {
            string DestinationFolder = @"C:\DVLD_ProjectImage\";
            if(!CreateFolderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }
            string DestinationFile = DestinationFolder + ReplaceFileNameByGuid(SourceFile);
            try
            {
                File.Copy(SourceFile, DestinationFile, true);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Can Not Copy File " + ex.Message);
                return false;
            }

            SourceFile = DestinationFile;
            return true;
                
        }
    }
}
