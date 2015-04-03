using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace USATodayBookList.Controls
{
    public partial class LinkableTextBox : System.Web.UI.UserControl
    {
        public event EventHandler onSaveClick;
        private string _CustomControlToValidate;

        public string CustomControlToValidate
        {
            get { return this._CustomControlToValidate; }
            set { this._CustomControlToValidate = value; }
        }

        public string LinkText
        {
            set { this.labelLink.Text = value; }
        }
        public string Text
        {
            get { return this.txtbox.Text; }
            set { this.txtbox.Text = value; }
        }
        public string InitialValue
        {
            set { this.RFV1.InitialValue = value; }
        }
        public string ErrorMessage
        {
            set { this.RFV1.ErrorMessage = value; }
        }

        public string CustomErrorMessage
        {
            set { this.CV1.ErrorMessage = value; }
        }
        public string CustomClientValidationFunction
        {
            set { this.CV1.ClientValidationFunction = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.labelLink.Attributes.Add("onclick", "show('" + this.divTextBox.ClientID + "');");
                this.labelLink.Attributes.Add("style", "cursor:hand;cursor:pointer;");
                this.RFV1.ValidationGroup = this.labelLink.ClientID;
                this.btnSave.ValidationGroup = this.labelLink.ClientID;
                this.btnSave.CausesValidation = true;

                if (this._CustomControlToValidate != string.Empty)
                {
                    this.CV1.Visible = true;
                    this.CV1.ValidationGroup = this.labelLink.ClientID;                
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
           this.btnSave.Click +=new EventHandler(btnSave_Click);
            base.OnInit(e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            onSaveClick(sender, e);
        }
    }
}