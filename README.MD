# 微信公众号开发 #
---
## 1. 验证签名 ##
	WeiXinSignature.ValidateSignature()

## 2. 获取TOKEN ##
	WxAccessTokenService.GetToken()

appSettings配置项：

- IsDevelop：配置当前是否处理开发阶段
- WxToken：开发阶段使用的TOKEN

## 3. 生成临时二维码 ##
	WxAccountService.GetQrCode(int sceneId)

- screen: 生成二维码的场景

## 4. 发送客服消息 ##
	CustomerService.Send(CustomerMessage message)
- message: 客户消息对象

## 5. 微信服务器事件推送处理 ##
1. 自定义菜单事件
2. 微信推荐事件