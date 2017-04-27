﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace OrderImportClasses.SCTWS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Net;

    /// <remarks/>
    // CODEGEN: The optional WSDL extension element 'Policy' from namespace 'http://schemas.xmlsoap.org/ws/2004/09/policy' was not handled.
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="zsd_create_web_so", Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class zsd_create_web_so : System.Web.Services.Protocols.SoapHttpClientProtocol {

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var webRequest = (HttpWebRequest)base.GetWebRequest(uri);
            webRequest.KeepAlive = false;
            return webRequest;
        }

        private System.Threading.SendOrPostCallback ZSD_CREATE_WEB_SALES_ORDEROperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public zsd_create_web_so() {
            this.Url = global::OrderImportClasses.Properties.Settings.Default.OrderImportClasses_SCTWS_zsd_create_web_so;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ZSD_CREATE_WEB_SALES_ORDERCompletedEventHandler ZSD_CREATE_WEB_SALES_ORDERCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:sap-com:document:sap:rfc:functions:ZSD_CREATE_WEB_SO:ZSD_CREATE_WEB_SALES_ORD" +
            "ERRequest", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ZSD_CREATE_WEB_SALES_ORDERResponse", Namespace="urn:sap-com:document:sap:rfc:functions")]
        public ZSD_CREATE_WEB_SALES_ORDERResponse ZSD_CREATE_WEB_SALES_ORDER([System.Xml.Serialization.XmlElementAttribute("ZSD_CREATE_WEB_SALES_ORDER", Namespace="urn:sap-com:document:sap:rfc:functions")] ZSD_CREATE_WEB_SALES_ORDER ZSD_CREATE_WEB_SALES_ORDER1) {
            object[] results = this.Invoke("ZSD_CREATE_WEB_SALES_ORDER", new object[] {
                        ZSD_CREATE_WEB_SALES_ORDER1});
            return ((ZSD_CREATE_WEB_SALES_ORDERResponse)(results[0]));
        }
        
        /// <remarks/>
        public void ZSD_CREATE_WEB_SALES_ORDERAsync(ZSD_CREATE_WEB_SALES_ORDER ZSD_CREATE_WEB_SALES_ORDER1) {
            this.ZSD_CREATE_WEB_SALES_ORDERAsync(ZSD_CREATE_WEB_SALES_ORDER1, null);
        }
        
        /// <remarks/>
        public void ZSD_CREATE_WEB_SALES_ORDERAsync(ZSD_CREATE_WEB_SALES_ORDER ZSD_CREATE_WEB_SALES_ORDER1, object userState) {
            if ((this.ZSD_CREATE_WEB_SALES_ORDEROperationCompleted == null)) {
                this.ZSD_CREATE_WEB_SALES_ORDEROperationCompleted = new System.Threading.SendOrPostCallback(this.OnZSD_CREATE_WEB_SALES_ORDEROperationCompleted);
            }
            this.InvokeAsync("ZSD_CREATE_WEB_SALES_ORDER", new object[] {
                        ZSD_CREATE_WEB_SALES_ORDER1}, this.ZSD_CREATE_WEB_SALES_ORDEROperationCompleted, userState);
        }
        
        private void OnZSD_CREATE_WEB_SALES_ORDEROperationCompleted(object arg) {
            if ((this.ZSD_CREATE_WEB_SALES_ORDERCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ZSD_CREATE_WEB_SALES_ORDERCompleted(this, new ZSD_CREATE_WEB_SALES_ORDERCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSD_CREATE_WEB_SALES_ORDER {
        
        private BAPIRET2[] eT_RETURNField;
        
        private ZSD_WEB_SO_HEADER_S iS_SO_HEADERField;
        
        private ZSD_WEB_SO_ITEM_S[] iT_SO_ITEMField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public BAPIRET2[] ET_RETURN {
            get {
                return this.eT_RETURNField;
            }
            set {
                this.eT_RETURNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ZSD_WEB_SO_HEADER_S IS_SO_HEADER {
            get {
                return this.iS_SO_HEADERField;
            }
            set {
                this.iS_SO_HEADERField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZSD_WEB_SO_ITEM_S[] IT_SO_ITEM {
            get {
                return this.iT_SO_ITEMField;
            }
            set {
                this.iT_SO_ITEMField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class BAPIRET2 {
        
        private string tYPEField;
        
        private string idField;
        
        private string nUMBERField;
        
        private string mESSAGEField;
        
        private string lOG_NOField;
        
        private string lOG_MSG_NOField;
        
        private string mESSAGE_V1Field;
        
        private string mESSAGE_V2Field;
        
        private string mESSAGE_V3Field;
        
        private string mESSAGE_V4Field;
        
        private string pARAMETERField;
        
        private int rOWField;
        
        private string fIELDField;
        
        private string sYSTEMField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string TYPE {
            get {
                return this.tYPEField;
            }
            set {
                this.tYPEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ID {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string NUMBER {
            get {
                return this.nUMBERField;
            }
            set {
                this.nUMBERField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MESSAGE {
            get {
                return this.mESSAGEField;
            }
            set {
                this.mESSAGEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LOG_NO {
            get {
                return this.lOG_NOField;
            }
            set {
                this.lOG_NOField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LOG_MSG_NO {
            get {
                return this.lOG_MSG_NOField;
            }
            set {
                this.lOG_MSG_NOField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MESSAGE_V1 {
            get {
                return this.mESSAGE_V1Field;
            }
            set {
                this.mESSAGE_V1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MESSAGE_V2 {
            get {
                return this.mESSAGE_V2Field;
            }
            set {
                this.mESSAGE_V2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MESSAGE_V3 {
            get {
                return this.mESSAGE_V3Field;
            }
            set {
                this.mESSAGE_V3Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MESSAGE_V4 {
            get {
                return this.mESSAGE_V4Field;
            }
            set {
                this.mESSAGE_V4Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PARAMETER {
            get {
                return this.pARAMETERField;
            }
            set {
                this.pARAMETERField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int ROW {
            get {
                return this.rOWField;
            }
            set {
                this.rOWField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FIELD {
            get {
                return this.fIELDField;
            }
            set {
                this.fIELDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SYSTEM {
            get {
                return this.sYSTEMField;
            }
            set {
                this.sYSTEMField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSD_WEB_SO_ITEM_S {
        
        private string mATNRField;
        
        private decimal kWMENGField;
        
        private string mEINSField;
        
        private decimal nTGEWField;
        
        private decimal bRGEWField;
        
        private decimal vOLUMField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MATNR {
            get {
                return this.mATNRField;
            }
            set {
                this.mATNRField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal KWMENG {
            get {
                return this.kWMENGField;
            }
            set {
                this.kWMENGField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MEINS {
            get {
                return this.mEINSField;
            }
            set {
                this.mEINSField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal NTGEW {
            get {
                return this.nTGEWField;
            }
            set {
                this.nTGEWField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal BRGEW {
            get {
                return this.bRGEWField;
            }
            set {
                this.bRGEWField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal VOLUM {
            get {
                return this.vOLUMField;
            }
            set {
                this.vOLUMField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSD_WEB_SO_HEADER_S {
        
        private string bSTKDField;
        
        private string bSTKD_EField;
        
        private string tEMPField;
        
        private string tRATYField;
        
        private string kUNAGField;
        
        private string sHIPFRField;
        
        private string bSTDKField;
        
        private string pICKUPField;
        
        private string sHIPTOField;
        
        private string vDATUField;
        
        private string dELCOField;
        
        private string dELIV_INSTRField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string BSTKD {
            get {
                return this.bSTKDField;
            }
            set {
                this.bSTKDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string BSTKD_E {
            get {
                return this.bSTKD_EField;
            }
            set {
                this.bSTKD_EField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string TEMP {
            get {
                return this.tEMPField;
            }
            set {
                this.tEMPField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string TRATY {
            get {
                return this.tRATYField;
            }
            set {
                this.tRATYField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string KUNAG {
            get {
                return this.kUNAGField;
            }
            set {
                this.kUNAGField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SHIPFR {
            get {
                return this.sHIPFRField;
            }
            set {
                this.sHIPFRField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string BSTDK {
            get {
                return this.bSTDKField;
            }
            set {
                this.bSTDKField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PICKUP {
            get {
                return this.pICKUPField;
            }
            set {
                this.pICKUPField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SHIPTO {
            get {
                return this.sHIPTOField;
            }
            set {
                this.sHIPTOField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string VDATU {
            get {
                return this.vDATUField;
            }
            set {
                this.vDATUField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string DELCO {
            get {
                return this.dELCOField;
            }
            set {
                this.dELCOField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string DELIV_INSTR {
            get {
                return this.dELIV_INSTRField;
            }
            set {
                this.dELIV_INSTRField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:rfc:functions")]
    public partial class ZSD_CREATE_WEB_SALES_ORDERResponse {
        
        private BAPIRET2[] eT_RETURNField;
        
        private ZSD_WEB_SO_ITEM_S[] iT_SO_ITEMField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public BAPIRET2[] ET_RETURN {
            get {
                return this.eT_RETURNField;
            }
            set {
                this.eT_RETURNField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public ZSD_WEB_SO_ITEM_S[] IT_SO_ITEM {
            get {
                return this.iT_SO_ITEMField;
            }
            set {
                this.iT_SO_ITEMField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    public delegate void ZSD_CREATE_WEB_SALES_ORDERCompletedEventHandler(object sender, ZSD_CREATE_WEB_SALES_ORDERCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1586.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ZSD_CREATE_WEB_SALES_ORDERCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ZSD_CREATE_WEB_SALES_ORDERCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ZSD_CREATE_WEB_SALES_ORDERResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ZSD_CREATE_WEB_SALES_ORDERResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591