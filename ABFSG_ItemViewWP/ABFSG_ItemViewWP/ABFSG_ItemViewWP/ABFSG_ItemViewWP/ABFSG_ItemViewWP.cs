using System;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Serialization;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

namespace ABFSG_ItemViewWP
{
    [Guid("dd026c54-a7ba-4181-ab1b-6ebbbd333d0e")]
    public class ABFSG_ItemViewWP : System.Web.UI.WebControls.WebParts.WebPart
    {
        private System.Web.UI.ScriptManager _AjaxManager;
        UserControl userControlItemView;
        public string strControlLink=string.Empty;
        public ABFSG_ItemViewWP()
        {
        }
        [WebPartStorage(Storage.None)]
        public ScriptManager AjaxManager
        {
            get { return _AjaxManager; }
            set { _AjaxManager = value; }
        }
        [Personalizable(PersonalizationScope.User), WebBrowsable, WebDisplayName("Enter the path to the User Control.\n EX: /_controltemplates/ABFSG/Filename.ascx"), WebDescription("Use this property to change the path to User Control.")]
        public string UserControlURL
        {
            get { return strControlLink; }
            set { strControlLink = value; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _AjaxManager = ScriptManager.GetCurrent(this.Page);
            if (_AjaxManager == null)
            {

                _AjaxManager = new ScriptManager();
                _AjaxManager.EnablePartialRendering = true;
                _AjaxManager.EnableScriptLocalization = true;

                Page.ClientScript.RegisterStartupScript(this.GetType(), this.ID, "_spOriginalFormAction = document.forms[0].action;", true);
                if (this.Page.Form != null)
                {
                    string formOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                    if (!string.IsNullOrEmpty(formOnSubmitAtt) && formOnSubmitAtt == "return _spFormOnSubmitWrapper();")
                    {
                        this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                    }
                    this.Page.Form.Controls.AddAt(0, _AjaxManager);
                }
            }
        }        
        protected override void CreateChildControls()
        {
            try
            {
                this.Controls.Clear();
                userControlItemView = (UserControl)this.Page.LoadControl(UserControlURL);
                this.Controls.Add(userControlItemView);
            }
            catch (Exception ex)
            {
                this.Controls.Add(new LiteralControl(ex.Message));
            }
        }
    }
}
