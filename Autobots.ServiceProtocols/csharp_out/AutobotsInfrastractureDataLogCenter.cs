// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: protos/Autobots.Infrastracture.DataLogCenter.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Autobots.Infrastracture.DataLogCenter {

  /// <summary>Holder for reflection information generated from protos/Autobots.Infrastracture.DataLogCenter.proto</summary>
  public static partial class AutobotsInfrastractureDataLogCenterReflection {

    #region Descriptor
    /// <summary>File descriptor for protos/Autobots.Infrastracture.DataLogCenter.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static AutobotsInfrastractureDataLogCenterReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CjJwcm90b3MvQXV0b2JvdHMuSW5mcmFzdHJhY3R1cmUuRGF0YUxvZ0NlbnRl",
            "ci5wcm90bxIlQXV0b2JvdHMuSW5mcmFzdHJhY3R1cmUuRGF0YUxvZ0NlbnRl",
            "ciIeCg5EYXRhTG9nUmVxdWVzdBIMCgR0ZXh0GAEgASgJIrgBCg9EYXRhTG9n",
            "UmVzcG9uc2USVAoGc3RhdHVzGAEgASgOMkQuQXV0b2JvdHMuSW5mcmFzdHJh",
            "Y3R1cmUuRGF0YUxvZ0NlbnRlci5EYXRhTG9nUmVzcG9uc2UuU2VydmluZ1N0",
            "YXR1cyJPCg1TZXJ2aW5nU3RhdHVzEgsKB1VOS05PV04QABILCgdTRVJWSU5H",
            "EAESDwoLTk9UX1NFUlZJTkcQAhITCg9TRVJWSUNFX1VOS05PV04QAzKaAQoY",
            "RGF0YUxvZ1NlcnZpY2VEZWZpbml0aW9uEn4KDUNvbW1pdERhdGFMb2cSNS5B",
            "dXRvYm90cy5JbmZyYXN0cmFjdHVyZS5EYXRhTG9nQ2VudGVyLkRhdGFMb2dS",
            "ZXF1ZXN0GjYuQXV0b2JvdHMuSW5mcmFzdHJhY3R1cmUuRGF0YUxvZ0NlbnRl",
            "ci5EYXRhTG9nUmVzcG9uc2ViBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Autobots.Infrastracture.DataLogCenter.DataLogRequest), global::Autobots.Infrastracture.DataLogCenter.DataLogRequest.Parser, new[]{ "Text" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Autobots.Infrastracture.DataLogCenter.DataLogResponse), global::Autobots.Infrastracture.DataLogCenter.DataLogResponse.Parser, new[]{ "Status" }, null, new[]{ typeof(global::Autobots.Infrastracture.DataLogCenter.DataLogResponse.Types.ServingStatus) }, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class DataLogRequest : pb::IMessage<DataLogRequest> {
    private static readonly pb::MessageParser<DataLogRequest> _parser = new pb::MessageParser<DataLogRequest>(() => new DataLogRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DataLogRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Autobots.Infrastracture.DataLogCenter.AutobotsInfrastractureDataLogCenterReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataLogRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataLogRequest(DataLogRequest other) : this() {
      text_ = other.text_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataLogRequest Clone() {
      return new DataLogRequest(this);
    }

    /// <summary>Field number for the "text" field.</summary>
    public const int TextFieldNumber = 1;
    private string text_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Text {
      get { return text_; }
      set {
        text_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DataLogRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DataLogRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Text != other.Text) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Text.Length != 0) hash ^= Text.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Text.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Text);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Text.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Text);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DataLogRequest other) {
      if (other == null) {
        return;
      }
      if (other.Text.Length != 0) {
        Text = other.Text;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Text = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class DataLogResponse : pb::IMessage<DataLogResponse> {
    private static readonly pb::MessageParser<DataLogResponse> _parser = new pb::MessageParser<DataLogResponse>(() => new DataLogResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DataLogResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Autobots.Infrastracture.DataLogCenter.AutobotsInfrastractureDataLogCenterReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataLogResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataLogResponse(DataLogResponse other) : this() {
      status_ = other.status_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataLogResponse Clone() {
      return new DataLogResponse(this);
    }

    /// <summary>Field number for the "status" field.</summary>
    public const int StatusFieldNumber = 1;
    private global::Autobots.Infrastracture.DataLogCenter.DataLogResponse.Types.ServingStatus status_ = global::Autobots.Infrastracture.DataLogCenter.DataLogResponse.Types.ServingStatus.Unknown;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Autobots.Infrastracture.DataLogCenter.DataLogResponse.Types.ServingStatus Status {
      get { return status_; }
      set {
        status_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DataLogResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DataLogResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Status != other.Status) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Status != global::Autobots.Infrastracture.DataLogCenter.DataLogResponse.Types.ServingStatus.Unknown) hash ^= Status.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Status != global::Autobots.Infrastracture.DataLogCenter.DataLogResponse.Types.ServingStatus.Unknown) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Status);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Status != global::Autobots.Infrastracture.DataLogCenter.DataLogResponse.Types.ServingStatus.Unknown) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Status);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DataLogResponse other) {
      if (other == null) {
        return;
      }
      if (other.Status != global::Autobots.Infrastracture.DataLogCenter.DataLogResponse.Types.ServingStatus.Unknown) {
        Status = other.Status;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Status = (global::Autobots.Infrastracture.DataLogCenter.DataLogResponse.Types.ServingStatus) input.ReadEnum();
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the DataLogResponse message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public enum ServingStatus {
        [pbr::OriginalName("UNKNOWN")] Unknown = 0,
        [pbr::OriginalName("SERVING")] Serving = 1,
        [pbr::OriginalName("NOT_SERVING")] NotServing = 2,
        /// <summary>
        /// Used only by the Watch method.
        /// </summary>
        [pbr::OriginalName("SERVICE_UNKNOWN")] ServiceUnknown = 3,
      }

    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code