using ProtoBuf;

namespace WebApi.Client.Dtos;


[ProtoContract]
public class HelloDto
{
    [ProtoMember(1)]
    public string FirstName { get; set; }
    [ProtoMember(2)]
    public string LastName { get; set; }
}