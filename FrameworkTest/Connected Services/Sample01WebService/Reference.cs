﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FrameworkTest.Sample01WebService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="HelloRequest", Namespace="http://microsoft.com/webservices/")]
    [System.SerializableAttribute()]
    public partial class HelloRequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://microsoft.com/webservices/", ConfigurationName="Sample01WebService.Sample01WebServiceSoap")]
    public interface Sample01WebServiceSoap {
        
        // CODEGEN: 命名空间 http://microsoft.com/webservices/ 的元素名称 hello 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://microsoft.com/webservices/HelloWorld", ReplyAction="*")]
        FrameworkTest.Sample01WebService.HelloWorldResponse HelloWorld(FrameworkTest.Sample01WebService.HelloWorldRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://microsoft.com/webservices/HelloWorld", ReplyAction="*")]
        System.Threading.Tasks.Task<FrameworkTest.Sample01WebService.HelloWorldResponse> HelloWorldAsync(FrameworkTest.Sample01WebService.HelloWorldRequest request);
        
        // CODEGEN: 命名空间 http://microsoft.com/webservices/ 的元素名称 HelloCommonResult 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://microsoft.com/webservices/HelloCommon", ReplyAction="*")]
        FrameworkTest.Sample01WebService.HelloCommonResponse HelloCommon(FrameworkTest.Sample01WebService.HelloCommonRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://microsoft.com/webservices/HelloCommon", ReplyAction="*")]
        System.Threading.Tasks.Task<FrameworkTest.Sample01WebService.HelloCommonResponse> HelloCommonAsync(FrameworkTest.Sample01WebService.HelloCommonRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorld", Namespace="http://microsoft.com/webservices/", Order=0)]
        public FrameworkTest.Sample01WebService.HelloWorldRequestBody Body;
        
        public HelloWorldRequest() {
        }
        
        public HelloWorldRequest(FrameworkTest.Sample01WebService.HelloWorldRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://microsoft.com/webservices/")]
    public partial class HelloWorldRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public FrameworkTest.Sample01WebService.HelloRequest hello;
        
        public HelloWorldRequestBody() {
        }
        
        public HelloWorldRequestBody(FrameworkTest.Sample01WebService.HelloRequest hello) {
            this.hello = hello;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorldResponse", Namespace="http://microsoft.com/webservices/", Order=0)]
        public FrameworkTest.Sample01WebService.HelloWorldResponseBody Body;
        
        public HelloWorldResponse() {
        }
        
        public HelloWorldResponse(FrameworkTest.Sample01WebService.HelloWorldResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://microsoft.com/webservices/")]
    public partial class HelloWorldResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloWorldResult;
        
        public HelloWorldResponseBody() {
        }
        
        public HelloWorldResponseBody(string HelloWorldResult) {
            this.HelloWorldResult = HelloWorldResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloCommonRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloCommon", Namespace="http://microsoft.com/webservices/", Order=0)]
        public FrameworkTest.Sample01WebService.HelloCommonRequestBody Body;
        
        public HelloCommonRequest() {
        }
        
        public HelloCommonRequest(FrameworkTest.Sample01WebService.HelloCommonRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class HelloCommonRequestBody {
        
        public HelloCommonRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloCommonResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloCommonResponse", Namespace="http://microsoft.com/webservices/", Order=0)]
        public FrameworkTest.Sample01WebService.HelloCommonResponseBody Body;
        
        public HelloCommonResponse() {
        }
        
        public HelloCommonResponse(FrameworkTest.Sample01WebService.HelloCommonResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://microsoft.com/webservices/")]
    public partial class HelloCommonResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloCommonResult;
        
        public HelloCommonResponseBody() {
        }
        
        public HelloCommonResponseBody(string HelloCommonResult) {
            this.HelloCommonResult = HelloCommonResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface Sample01WebServiceSoapChannel : FrameworkTest.Sample01WebService.Sample01WebServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Sample01WebServiceSoapClient : System.ServiceModel.ClientBase<FrameworkTest.Sample01WebService.Sample01WebServiceSoap>, FrameworkTest.Sample01WebService.Sample01WebServiceSoap {
        
        public Sample01WebServiceSoapClient() {
        }
        
        public Sample01WebServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Sample01WebServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Sample01WebServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Sample01WebServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FrameworkTest.Sample01WebService.HelloWorldResponse FrameworkTest.Sample01WebService.Sample01WebServiceSoap.HelloWorld(FrameworkTest.Sample01WebService.HelloWorldRequest request) {
            return base.Channel.HelloWorld(request);
        }
        
        public string HelloWorld(FrameworkTest.Sample01WebService.HelloRequest hello) {
            FrameworkTest.Sample01WebService.HelloWorldRequest inValue = new FrameworkTest.Sample01WebService.HelloWorldRequest();
            inValue.Body = new FrameworkTest.Sample01WebService.HelloWorldRequestBody();
            inValue.Body.hello = hello;
            FrameworkTest.Sample01WebService.HelloWorldResponse retVal = ((FrameworkTest.Sample01WebService.Sample01WebServiceSoap)(this)).HelloWorld(inValue);
            return retVal.Body.HelloWorldResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<FrameworkTest.Sample01WebService.HelloWorldResponse> FrameworkTest.Sample01WebService.Sample01WebServiceSoap.HelloWorldAsync(FrameworkTest.Sample01WebService.HelloWorldRequest request) {
            return base.Channel.HelloWorldAsync(request);
        }
        
        public System.Threading.Tasks.Task<FrameworkTest.Sample01WebService.HelloWorldResponse> HelloWorldAsync(FrameworkTest.Sample01WebService.HelloRequest hello) {
            FrameworkTest.Sample01WebService.HelloWorldRequest inValue = new FrameworkTest.Sample01WebService.HelloWorldRequest();
            inValue.Body = new FrameworkTest.Sample01WebService.HelloWorldRequestBody();
            inValue.Body.hello = hello;
            return ((FrameworkTest.Sample01WebService.Sample01WebServiceSoap)(this)).HelloWorldAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FrameworkTest.Sample01WebService.HelloCommonResponse FrameworkTest.Sample01WebService.Sample01WebServiceSoap.HelloCommon(FrameworkTest.Sample01WebService.HelloCommonRequest request) {
            return base.Channel.HelloCommon(request);
        }
        
        public string HelloCommon() {
            FrameworkTest.Sample01WebService.HelloCommonRequest inValue = new FrameworkTest.Sample01WebService.HelloCommonRequest();
            inValue.Body = new FrameworkTest.Sample01WebService.HelloCommonRequestBody();
            FrameworkTest.Sample01WebService.HelloCommonResponse retVal = ((FrameworkTest.Sample01WebService.Sample01WebServiceSoap)(this)).HelloCommon(inValue);
            return retVal.Body.HelloCommonResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<FrameworkTest.Sample01WebService.HelloCommonResponse> FrameworkTest.Sample01WebService.Sample01WebServiceSoap.HelloCommonAsync(FrameworkTest.Sample01WebService.HelloCommonRequest request) {
            return base.Channel.HelloCommonAsync(request);
        }
        
        public System.Threading.Tasks.Task<FrameworkTest.Sample01WebService.HelloCommonResponse> HelloCommonAsync() {
            FrameworkTest.Sample01WebService.HelloCommonRequest inValue = new FrameworkTest.Sample01WebService.HelloCommonRequest();
            inValue.Body = new FrameworkTest.Sample01WebService.HelloCommonRequestBody();
            return ((FrameworkTest.Sample01WebService.Sample01WebServiceSoap)(this)).HelloCommonAsync(inValue);
        }
    }
}
