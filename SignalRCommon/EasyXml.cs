using Newtonsoft.Json;
using SignalRModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace SignalRCommon
{
    /// <summary>
    /// XML帮助类
    /// </summary>
    public class EasyXml
    {

        private string filePath = Path.Combine(Directory.GetCurrentDirectory(), "LocalData.xml");
        /// <summary>
        /// 创建XML
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="content">内容</param>
        /// <param name="datetimeStr">时间戳</param>
        private void CreatDoc(UserInfor user)
        {
            XmlDocument doc = new XmlDocument();
            //3、创建第一个行描述信息，并且添加到doc文档中
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //4、创建根节点
            XmlElement books = doc.CreateElement("UserInfors");
            //将根节点添加到文档中
            doc.AppendChild(books);

            //5、给根节点Books创建子节点
            XmlElement book = doc.CreateElement("UserInfor");
            //将book添加到根节点
            books.AppendChild(book);
            //6、给Book1添加子节点
            XmlElement name = doc.CreateElement("Infor");
            name.SetAttribute("ClientId", user.ClientId);
            name.SetAttribute("User", user.User);
            book.AppendChild(name);

            doc.Save(filePath);
        }

        public void WriteDoc(string infoJson)
        {
            string SavePath = filePath;
            UserInfor info = JsonConvert.DeserializeObject<UserInfor>(infoJson);
            //判断是否存在文件夹
            //var DirectoryPath = Path.GetDirectoryName(SavePath);  //获取文件夹所在的路径
            //if (!Directory.Exists(SavePath))
            //{
            //    Directory.CreateDirectory(SavePath);  //创建文件夹
            //}



            XmlDocument doc = new XmlDocument();
            if (File.Exists(filePath))
            {
                //如果文件存在 加载XML
                doc.Load(filePath);
                //获得文件的根节点
                XmlNodeList xnl = doc.SelectNodes("/UserInfors/UserInfor/Infor");
                if (xnl.Count < 1)
                {
                    CreatDoc(info);
                }
                else
                {
                    XmlNode PNode = null;
                    var isHave = false;
                    foreach (XmlNode item in xnl)
                    {
                        PNode = item.ParentNode;
                        var name = item.Attributes["User"].Value;
                        if (name == info.User)
                        {
                            isHave = true;
                            item.Attributes["ClientId"].Value = info.ClientId;
                            break;
                        }
                    }
                    if (!isHave)
                    {
                        var en = doc.DocumentElement;
                        XmlElement name1 = doc.CreateElement("User");
                        name1.SetAttribute("ClientId", info.ClientId);
                        name1.SetAttribute("User", info.User);
                        PNode.AppendChild(name1);

                        if (xnl.Count > 20)
                        {
                            PNode.RemoveChild(xnl[0]);
                        }

                    }
                }
                doc.Save(filePath);
            }
            else
            {
                CreatDoc(info);
            }
        }

        /// <summary>
        /// 读取XML文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public UserInfor ReadDoc(string user)
        {
            UserInfor info = new UserInfor();
            XmlDocument doc = new XmlDocument();
            if (File.Exists(filePath))
            {
                //如果文件存在 加载XML
                doc.Load(filePath);
                //获得文件的根节点
                XmlNodeList xnl = doc.SelectNodes("/UserInfors/UserInfor/Infor");
                if (xnl.Count > 0)
                {
                    foreach (XmlNode item in xnl)
                    {
                        if (item.Attributes["User"].Value == user)
                        {
                            info.ClientId = item.Attributes["ClientId"].Value;
                            info.User = item.Attributes["User"].Value;
                        }
                    }
                }
            }

            return info;
        }

        public List<UserInfor> ReadDocs()
        {
            XmlDocument doc = new XmlDocument();
            List<UserInfor> infors = new List<UserInfor>();
            if (File.Exists(filePath))
            {
                doc.Load(filePath);
                XmlNodeList nodes = doc.SelectNodes("//Infor");

                foreach (XmlNode node in nodes)
                {
                    UserInfor info = new UserInfor();
                    info.ClientId = node.Attributes["ClientId"].Value;
                    info.User = node.Attributes["User"].Value;
                    infors.Add(info);
                }
            } 
            return infors; 
        }

    }

}
