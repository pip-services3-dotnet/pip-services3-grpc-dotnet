// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: commandable.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace PipServices3.Grpc.Services {

  /// <summary>Holder for reflection information generated from commandable.proto</summary>
  public static partial class CommandableReflection {

    #region Descriptor
    /// <summary>File descriptor for commandable.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CommandableReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChFjb21tYW5kYWJsZS5wcm90bxILY29tbWFuZGFibGUi/AEKEEVycm9yRGVz",
            "Y3JpcHRpb24SEAoIY2F0ZWdvcnkYASABKAkSDAoEY29kZRgCIAEoCRIWCg5j",
            "b3JyZWxhdGlvbl9pZBgDIAEoCRIOCgZzdGF0dXMYBCABKAUSDwoHbWVzc2Fn",
            "ZRgFIAEoCRINCgVjYXVzZRgGIAEoCRITCgtzdGFja190cmFjZRgHIAEoCRI7",
            "CgdkZXRhaWxzGAggAygLMiouY29tbWFuZGFibGUuRXJyb3JEZXNjcmlwdGlv",
            "bi5EZXRhaWxzRW50cnkaLgoMRGV0YWlsc0VudHJ5EgsKA2tleRgBIAEoCRIN",
            "CgV2YWx1ZRgCIAEoCToCOAEiXgoNSW52b2tlUmVxdWVzdBIOCgZtZXRob2QY",
            "ASABKAkSFgoOY29ycmVsYXRpb25faWQYAiABKAkSEgoKYXJnc19lbXB0eRgD",
            "IAEoCBIRCglhcmdzX2pzb24YBCABKAkiZgoLSW52b2tlUmVwbHkSLAoFZXJy",
            "b3IYASABKAsyHS5jb21tYW5kYWJsZS5FcnJvckRlc2NyaXB0aW9uEhQKDHJl",
            "c3VsdF9lbXB0eRgCIAEoCBITCgtyZXN1bHRfanNvbhgDIAEoCTJPCgtDb21t",
            "YW5kYWJsZRJACgZpbnZva2USGi5jb21tYW5kYWJsZS5JbnZva2VSZXF1ZXN0",
            "GhguY29tbWFuZGFibGUuSW52b2tlUmVwbHkiAEJjCh1waXAtc2VydmljZXMu",
            "Z3JwYy5jb21tYW5kYWJsZUIQQ29tbWFuZGFibGVQcm90b1ABWgZwcm90b3Oi",
            "AghHUlBDX0NNRKoCGlBpcFNlcnZpY2VzMy5HcnBjLlNlcnZpY2VzYgZwcm90",
            "bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::PipServices3.Grpc.Services.ErrorDescription), global::PipServices3.Grpc.Services.ErrorDescription.Parser, new[]{ "Category", "Code", "CorrelationId", "Status", "Message", "Cause", "StackTrace", "Details" }, null, null, null, new pbr::GeneratedClrTypeInfo[] { null, }),
            new pbr::GeneratedClrTypeInfo(typeof(global::PipServices3.Grpc.Services.InvokeRequest), global::PipServices3.Grpc.Services.InvokeRequest.Parser, new[]{ "Method", "CorrelationId", "ArgsEmpty", "ArgsJson" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::PipServices3.Grpc.Services.InvokeReply), global::PipServices3.Grpc.Services.InvokeReply.Parser, new[]{ "Error", "ResultEmpty", "ResultJson" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class ErrorDescription : pb::IMessage<ErrorDescription> {
    private static readonly pb::MessageParser<ErrorDescription> _parser = new pb::MessageParser<ErrorDescription>(() => new ErrorDescription());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ErrorDescription> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::PipServices3.Grpc.Services.CommandableReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ErrorDescription() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ErrorDescription(ErrorDescription other) : this() {
      category_ = other.category_;
      code_ = other.code_;
      correlationId_ = other.correlationId_;
      status_ = other.status_;
      message_ = other.message_;
      cause_ = other.cause_;
      stackTrace_ = other.stackTrace_;
      details_ = other.details_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ErrorDescription Clone() {
      return new ErrorDescription(this);
    }

    /// <summary>Field number for the "category" field.</summary>
    public const int CategoryFieldNumber = 1;
    private string category_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Category {
      get { return category_; }
      set {
        category_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "code" field.</summary>
    public const int CodeFieldNumber = 2;
    private string code_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Code {
      get { return code_; }
      set {
        code_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "correlation_id" field.</summary>
    public const int CorrelationIdFieldNumber = 3;
    private string correlationId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string CorrelationId {
      get { return correlationId_; }
      set {
        correlationId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "status" field.</summary>
    public const int StatusFieldNumber = 4;
    private int status_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Status {
      get { return status_; }
      set {
        status_ = value;
      }
    }

    /// <summary>Field number for the "message" field.</summary>
    public const int MessageFieldNumber = 5;
    private string message_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Message {
      get { return message_; }
      set {
        message_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "cause" field.</summary>
    public const int CauseFieldNumber = 6;
    private string cause_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Cause {
      get { return cause_; }
      set {
        cause_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "stack_trace" field.</summary>
    public const int StackTraceFieldNumber = 7;
    private string stackTrace_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string StackTrace {
      get { return stackTrace_; }
      set {
        stackTrace_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "details" field.</summary>
    public const int DetailsFieldNumber = 8;
    private static readonly pbc::MapField<string, string>.Codec _map_details_codec
        = new pbc::MapField<string, string>.Codec(pb::FieldCodec.ForString(10, ""), pb::FieldCodec.ForString(18, ""), 66);
    private readonly pbc::MapField<string, string> details_ = new pbc::MapField<string, string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<string, string> Details {
      get { return details_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ErrorDescription);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ErrorDescription other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Category != other.Category) return false;
      if (Code != other.Code) return false;
      if (CorrelationId != other.CorrelationId) return false;
      if (Status != other.Status) return false;
      if (Message != other.Message) return false;
      if (Cause != other.Cause) return false;
      if (StackTrace != other.StackTrace) return false;
      if (!Details.Equals(other.Details)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Category.Length != 0) hash ^= Category.GetHashCode();
      if (Code.Length != 0) hash ^= Code.GetHashCode();
      if (CorrelationId.Length != 0) hash ^= CorrelationId.GetHashCode();
      if (Status != 0) hash ^= Status.GetHashCode();
      if (Message.Length != 0) hash ^= Message.GetHashCode();
      if (Cause.Length != 0) hash ^= Cause.GetHashCode();
      if (StackTrace.Length != 0) hash ^= StackTrace.GetHashCode();
      hash ^= Details.GetHashCode();
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
      if (Category.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Category);
      }
      if (Code.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Code);
      }
      if (CorrelationId.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(CorrelationId);
      }
      if (Status != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(Status);
      }
      if (Message.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Message);
      }
      if (Cause.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(Cause);
      }
      if (StackTrace.Length != 0) {
        output.WriteRawTag(58);
        output.WriteString(StackTrace);
      }
      details_.WriteTo(output, _map_details_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Category.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Category);
      }
      if (Code.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Code);
      }
      if (CorrelationId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(CorrelationId);
      }
      if (Status != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Status);
      }
      if (Message.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Message);
      }
      if (Cause.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Cause);
      }
      if (StackTrace.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(StackTrace);
      }
      size += details_.CalculateSize(_map_details_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ErrorDescription other) {
      if (other == null) {
        return;
      }
      if (other.Category.Length != 0) {
        Category = other.Category;
      }
      if (other.Code.Length != 0) {
        Code = other.Code;
      }
      if (other.CorrelationId.Length != 0) {
        CorrelationId = other.CorrelationId;
      }
      if (other.Status != 0) {
        Status = other.Status;
      }
      if (other.Message.Length != 0) {
        Message = other.Message;
      }
      if (other.Cause.Length != 0) {
        Cause = other.Cause;
      }
      if (other.StackTrace.Length != 0) {
        StackTrace = other.StackTrace;
      }
      details_.Add(other.details_);
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
            Category = input.ReadString();
            break;
          }
          case 18: {
            Code = input.ReadString();
            break;
          }
          case 26: {
            CorrelationId = input.ReadString();
            break;
          }
          case 32: {
            Status = input.ReadInt32();
            break;
          }
          case 42: {
            Message = input.ReadString();
            break;
          }
          case 50: {
            Cause = input.ReadString();
            break;
          }
          case 58: {
            StackTrace = input.ReadString();
            break;
          }
          case 66: {
            details_.AddEntriesFrom(input, _map_details_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// The request message containing the invocation request.
  /// </summary>
  public sealed partial class InvokeRequest : pb::IMessage<InvokeRequest> {
    private static readonly pb::MessageParser<InvokeRequest> _parser = new pb::MessageParser<InvokeRequest>(() => new InvokeRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<InvokeRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::PipServices3.Grpc.Services.CommandableReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public InvokeRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public InvokeRequest(InvokeRequest other) : this() {
      method_ = other.method_;
      correlationId_ = other.correlationId_;
      argsEmpty_ = other.argsEmpty_;
      argsJson_ = other.argsJson_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public InvokeRequest Clone() {
      return new InvokeRequest(this);
    }

    /// <summary>Field number for the "method" field.</summary>
    public const int MethodFieldNumber = 1;
    private string method_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Method {
      get { return method_; }
      set {
        method_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "correlation_id" field.</summary>
    public const int CorrelationIdFieldNumber = 2;
    private string correlationId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string CorrelationId {
      get { return correlationId_; }
      set {
        correlationId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "args_empty" field.</summary>
    public const int ArgsEmptyFieldNumber = 3;
    private bool argsEmpty_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool ArgsEmpty {
      get { return argsEmpty_; }
      set {
        argsEmpty_ = value;
      }
    }

    /// <summary>Field number for the "args_json" field.</summary>
    public const int ArgsJsonFieldNumber = 4;
    private string argsJson_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ArgsJson {
      get { return argsJson_; }
      set {
        argsJson_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as InvokeRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(InvokeRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Method != other.Method) return false;
      if (CorrelationId != other.CorrelationId) return false;
      if (ArgsEmpty != other.ArgsEmpty) return false;
      if (ArgsJson != other.ArgsJson) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Method.Length != 0) hash ^= Method.GetHashCode();
      if (CorrelationId.Length != 0) hash ^= CorrelationId.GetHashCode();
      if (ArgsEmpty != false) hash ^= ArgsEmpty.GetHashCode();
      if (ArgsJson.Length != 0) hash ^= ArgsJson.GetHashCode();
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
      if (Method.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Method);
      }
      if (CorrelationId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(CorrelationId);
      }
      if (ArgsEmpty != false) {
        output.WriteRawTag(24);
        output.WriteBool(ArgsEmpty);
      }
      if (ArgsJson.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(ArgsJson);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Method.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Method);
      }
      if (CorrelationId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(CorrelationId);
      }
      if (ArgsEmpty != false) {
        size += 1 + 1;
      }
      if (ArgsJson.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ArgsJson);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(InvokeRequest other) {
      if (other == null) {
        return;
      }
      if (other.Method.Length != 0) {
        Method = other.Method;
      }
      if (other.CorrelationId.Length != 0) {
        CorrelationId = other.CorrelationId;
      }
      if (other.ArgsEmpty != false) {
        ArgsEmpty = other.ArgsEmpty;
      }
      if (other.ArgsJson.Length != 0) {
        ArgsJson = other.ArgsJson;
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
            Method = input.ReadString();
            break;
          }
          case 18: {
            CorrelationId = input.ReadString();
            break;
          }
          case 24: {
            ArgsEmpty = input.ReadBool();
            break;
          }
          case 34: {
            ArgsJson = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// The response message containing the invocation response
  /// </summary>
  public sealed partial class InvokeReply : pb::IMessage<InvokeReply> {
    private static readonly pb::MessageParser<InvokeReply> _parser = new pb::MessageParser<InvokeReply>(() => new InvokeReply());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<InvokeReply> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::PipServices3.Grpc.Services.CommandableReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public InvokeReply() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public InvokeReply(InvokeReply other) : this() {
      error_ = other.error_ != null ? other.error_.Clone() : null;
      resultEmpty_ = other.resultEmpty_;
      resultJson_ = other.resultJson_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public InvokeReply Clone() {
      return new InvokeReply(this);
    }

    /// <summary>Field number for the "error" field.</summary>
    public const int ErrorFieldNumber = 1;
    private global::PipServices3.Grpc.Services.ErrorDescription error_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::PipServices3.Grpc.Services.ErrorDescription Error {
      get { return error_; }
      set {
        error_ = value;
      }
    }

    /// <summary>Field number for the "result_empty" field.</summary>
    public const int ResultEmptyFieldNumber = 2;
    private bool resultEmpty_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool ResultEmpty {
      get { return resultEmpty_; }
      set {
        resultEmpty_ = value;
      }
    }

    /// <summary>Field number for the "result_json" field.</summary>
    public const int ResultJsonFieldNumber = 3;
    private string resultJson_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ResultJson {
      get { return resultJson_; }
      set {
        resultJson_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as InvokeReply);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(InvokeReply other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Error, other.Error)) return false;
      if (ResultEmpty != other.ResultEmpty) return false;
      if (ResultJson != other.ResultJson) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (error_ != null) hash ^= Error.GetHashCode();
      if (ResultEmpty != false) hash ^= ResultEmpty.GetHashCode();
      if (ResultJson.Length != 0) hash ^= ResultJson.GetHashCode();
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
      if (error_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Error);
      }
      if (ResultEmpty != false) {
        output.WriteRawTag(16);
        output.WriteBool(ResultEmpty);
      }
      if (ResultJson.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(ResultJson);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (error_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Error);
      }
      if (ResultEmpty != false) {
        size += 1 + 1;
      }
      if (ResultJson.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ResultJson);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(InvokeReply other) {
      if (other == null) {
        return;
      }
      if (other.error_ != null) {
        if (error_ == null) {
          Error = new global::PipServices3.Grpc.Services.ErrorDescription();
        }
        Error.MergeFrom(other.Error);
      }
      if (other.ResultEmpty != false) {
        ResultEmpty = other.ResultEmpty;
      }
      if (other.ResultJson.Length != 0) {
        ResultJson = other.ResultJson;
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
            if (error_ == null) {
              Error = new global::PipServices3.Grpc.Services.ErrorDescription();
            }
            input.ReadMessage(Error);
            break;
          }
          case 16: {
            ResultEmpty = input.ReadBool();
            break;
          }
          case 26: {
            ResultJson = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
