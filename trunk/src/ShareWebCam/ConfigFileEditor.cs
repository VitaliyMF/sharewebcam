#region License
/*
 * ShareWebCam utility (http://sharewebcam.googlecode.com/)
 * Copyright 2007 Vitaliy Fedorchenko
 * Distributed under the LGPL licence
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using System;
using System.Xml;
using System.IO;
using System.Drawing;

namespace ShareWebCam
{
	/// <summary>
	/// Operations with exe-config file.
	/// </summary>
	public class ConfigFileEditor
	{
		protected string fullConfigFilePath = null;
		
	
		public ConfigFileEditor(string fullConfigFilePath) {
			this.fullConfigFilePath = fullConfigFilePath;
		}
		
		public void SetRectangle(string key, Rectangle r) {
			SetValue(key, String.Format("{0},{1};{2},{3}", r.X, r.Y, r.Width, r.Height) );
		}

		public Rectangle GetRectangle(string key, Rectangle defaultRectangle) {
			string strRect = GetValue(key);
			if (strRect!=null) {
				string[] strRectParts = strRect.Split(';',',');
				try {
					return new Rectangle(
							Int32.Parse(strRectParts[0]),
							Int32.Parse(strRectParts[1]),
							Int32.Parse(strRectParts[2]),
							Int32.Parse(strRectParts[3])
						);
				} catch { /* ignore error */ }
			}
			return defaultRectangle;
		}

		/// <summary>
		/// Set new setting value
		/// </summary>
		/// <param name="key">setting key</param>
		/// <param name="val">setting new value</param>
		/// <returns>success flag</returns>
		public void SetValue(string key, string val) {
			XmlDocument xmlDoc = new XmlDocument();
			try {
				// read config XML document
				StreamReader fReader = new StreamReader(this.fullConfigFilePath);
				xmlDoc.Load(fReader);
				fReader.Close();
				
				// find section
				XmlNodeList nodesList = xmlDoc.SelectNodes("/configuration/appSettings");
				XmlNode appSettingsNode = nodesList[0]; // node should be, else exceptions generated
				
				// entry already exists?
				bool alreadyExists = false;
				foreach (XmlNode node in appSettingsNode.SelectNodes("./add") )
					if (node.Attributes["key"]!=null && node.Attributes["key"].Value==key) {
						if (node.Attributes["value"]==null)
							node.Attributes.Append( xmlDoc.CreateAttribute("value") );
						node.Attributes["value"].Value = val;
						alreadyExists = true;
					}
				
				if (!alreadyExists) {
					// create new entry
					XmlNode entryNode = xmlDoc.CreateNode(XmlNodeType.Element, "add", "");
					entryNode.Attributes.Append( xmlDoc.CreateAttribute("key") );
					entryNode.Attributes.Append( xmlDoc.CreateAttribute("value") );
					entryNode.Attributes["key"].Value = key;
					entryNode.Attributes["value"].Value = val;
					appSettingsNode.AppendChild(entryNode);
				}
				
				// write modified XML document
				StreamWriter fWriter = new StreamWriter(this.fullConfigFilePath);
				xmlDoc.Save(fWriter);
				fWriter.Close();
				
				// finally, refresh in-memory settings
				//System.Configuration.ConfigurationSettings.AppSettings[key] = val;
			} catch (Exception e) {
				System.Diagnostics.Trace.WriteLine("Cannot save setting: "+e.Message);
			}
		
		}
		
		/// <summary>
		/// Returns setting value
		/// </summary>
		/// <param name="key">setting key</param>
		/// <returns>string value or null (if key doesn't exists)</returns>
		public string GetValue(string key) {
			// read config XML document
			StreamReader fReader = new StreamReader(this.fullConfigFilePath);
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fReader);
			fReader.Close();
			
			// select node
			XmlNode paramNode = xmlDoc.SelectSingleNode(String.Format("/configuration/appSettings/add[@key=\"{0}\"]", key));
			return ( (paramNode!=null && paramNode.Attributes["value"]!=null) ? paramNode.Attributes["value"].Value : null);
		}
		
	}
}
