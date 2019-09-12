using AutoMapper;
using MockServer.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Niusys.Extensions.ResponseEnvelopes;
using Niusys.Utils;

namespace MockServer.ModelMappings
{
    public class EnvelopMessageWrapperResolver : IMemberValueResolver<object, object, string, string>, IValueResolver<ApiInterface, object, string>
    {
        public string Resolve(object source, object destination, string sourceMember, string destMember, ResolutionContext context)
        {
            var envelop = new EnvelopMessage<object>()
            {
                Code = 200,
                Tid = GuidGenerator.GenerateDigitalUUID(),
                FriendlyMessage = "返回成功",
                ErrorMessage = string.Empty
            };

            try
            {
                envelop.Data = JToken.Parse(sourceMember);
            }
            catch (System.Exception)
            {
                envelop.Data = "Json内容格式不正确, 通过编辑修复之后再查看";
            }
            return JsonConvert.SerializeObject(envelop, Formatting.Indented);
        }

        public string Resolve(ApiInterface source, object destination, string destMember, ResolutionContext context)
        {
            if (!source.IsUseEnvelop)
            {
                try
                {
                    var result = JToken.Parse(source.ResponseResult);
                    return JsonConvert.SerializeObject(result, Formatting.Indented);
                }
                catch (System.Exception)
                {
                    return "Json内容格式不正确, 通过编辑修复之后再查看";
                }
            }

            var envelop = new EnvelopMessage<object>()
            {
                Code = 200,
                Tid = GuidGenerator.GenerateDigitalUUID(),
                FriendlyMessage = "返回成功",
                ErrorMessage = string.Empty
            };

            try
            {
                envelop.Data = JToken.Parse(source.ResponseResult);
            }
            catch (System.Exception)
            {
                envelop.Data = "Json内容格式不正确, 通过编辑修复之后再查看";
            }
            return JsonConvert.SerializeObject(envelop, Formatting.Indented);
        }
    }
}
