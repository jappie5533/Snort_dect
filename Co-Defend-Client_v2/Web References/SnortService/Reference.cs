﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 原始程式碼已由 Microsoft.VSDesigner 自動產生，版本 4.0.30319.42000。
// 
#pragma warning disable 1591

namespace Co_Defend_Client_v2.SnortService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SnortSoap", Namespace="http://E502A_LAB.org/SnortAnalyze")]
    public partial class Snort : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback AnalyzeOperationCompleted;
        
        private System.Threading.SendOrPostCallback DownloadFileLenOperationCompleted;
        
        private System.Threading.SendOrPostCallback AnalysisResultOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Snort() {
            this.Url = global::Co_Defend_Client_v2.Properties.Settings.Default.Co_Defend_Client_v2_SnortService_SnortAnalyze;
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
        public event AnalyzeCompletedEventHandler AnalyzeCompleted;
        
        /// <remarks/>
        public event DownloadFileLenCompletedEventHandler DownloadFileLenCompleted;
        
        /// <remarks/>
        public event AnalysisResultCompletedEventHandler AnalysisResultCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://E502A_LAB.org/SnortAnalyze/Analyze", RequestNamespace="http://E502A_LAB.org/SnortAnalyze", ResponseNamespace="http://E502A_LAB.org/SnortAnalyze", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Analyze([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] fs, string fileName) {
            object[] results = this.Invoke("Analyze", new object[] {
                        fs,
                        fileName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void AnalyzeAsync(byte[] fs, string fileName) {
            this.AnalyzeAsync(fs, fileName, null);
        }
        
        /// <remarks/>
        public void AnalyzeAsync(byte[] fs, string fileName, object userState) {
            if ((this.AnalyzeOperationCompleted == null)) {
                this.AnalyzeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAnalyzeOperationCompleted);
            }
            this.InvokeAsync("Analyze", new object[] {
                        fs,
                        fileName}, this.AnalyzeOperationCompleted, userState);
        }
        
        private void OnAnalyzeOperationCompleted(object arg) {
            if ((this.AnalyzeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AnalyzeCompleted(this, new AnalyzeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://E502A_LAB.org/SnortAnalyze/DownloadFileLen", RequestNamespace="http://E502A_LAB.org/SnortAnalyze", ResponseNamespace="http://E502A_LAB.org/SnortAnalyze", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int DownloadFileLen(string fileName) {
            object[] results = this.Invoke("DownloadFileLen", new object[] {
                        fileName});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void DownloadFileLenAsync(string fileName) {
            this.DownloadFileLenAsync(fileName, null);
        }
        
        /// <remarks/>
        public void DownloadFileLenAsync(string fileName, object userState) {
            if ((this.DownloadFileLenOperationCompleted == null)) {
                this.DownloadFileLenOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDownloadFileLenOperationCompleted);
            }
            this.InvokeAsync("DownloadFileLen", new object[] {
                        fileName}, this.DownloadFileLenOperationCompleted, userState);
        }
        
        private void OnDownloadFileLenOperationCompleted(object arg) {
            if ((this.DownloadFileLenCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DownloadFileLenCompleted(this, new DownloadFileLenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://E502A_LAB.org/SnortAnalyze/AnalysisResult", RequestNamespace="http://E502A_LAB.org/SnortAnalyze", ResponseNamespace="http://E502A_LAB.org/SnortAnalyze", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] AnalysisResult(string fileName) {
            object[] results = this.Invoke("AnalysisResult", new object[] {
                        fileName});
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void AnalysisResultAsync(string fileName) {
            this.AnalysisResultAsync(fileName, null);
        }
        
        /// <remarks/>
        public void AnalysisResultAsync(string fileName, object userState) {
            if ((this.AnalysisResultOperationCompleted == null)) {
                this.AnalysisResultOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAnalysisResultOperationCompleted);
            }
            this.InvokeAsync("AnalysisResult", new object[] {
                        fileName}, this.AnalysisResultOperationCompleted, userState);
        }
        
        private void OnAnalysisResultOperationCompleted(object arg) {
            if ((this.AnalysisResultCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AnalysisResultCompleted(this, new AnalysisResultCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    public delegate void AnalyzeCompletedEventHandler(object sender, AnalyzeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AnalyzeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AnalyzeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    public delegate void DownloadFileLenCompletedEventHandler(object sender, DownloadFileLenCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DownloadFileLenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DownloadFileLenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    public delegate void AnalysisResultCompletedEventHandler(object sender, AnalysisResultCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1087.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AnalysisResultCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AnalysisResultCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591