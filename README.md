# Email Sender

Web API Rest para o envio de emails.

## Recursos
- Autorização de uso via chave privada por cliente cadastrado.
- Suporta múltiplos destinatários por mensagem.
- Não necessita de certificado SSL.

## Requisitos
- Para compilar e executar: *plataforma [.Net 5](https://dotnet.microsoft.com/download) ou superior*.
- Para explorar e alterar: *[Visual Studio](https://visualstudio.microsoft.com/pt-br/) ou outra IDE com suporte a projetos Web API do .Net 5*.
- Para testar: *ferramenta Rest como [Postman](https://www.postman.com) ou [Insomnia](https://insomnia.rest) ou cliente HTTP como [cURL](https://curl.se)*.

## Como Usar

1. Altere os parâmetros do arquivo *appsettings.json* de acordo com suas necessidades.
2. Execute o projeto em sua IDE ou compile-o e realize o deploy para um servidor HTTP.
3. Envie requisição HTTP POST, em sua ferramenta Rest, respeitando os parâmetros detalhados a seguir.

<br />
<p>
    <b>Parâmetros da Requisição</b>
</p>

Os cabeçalhos *POST*, *[Authorization](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Authorization)* e *Content-Type* precisam respeitar o formato abaixo.

```xml
POST /api/email HTTP/1.1
Authorization: ApiKey <chave_privada_do_cliente>
Content-Type: application/json
```

O corpo da mensagem deve ser construído da seguinte maneira:

```xml
{
"subject":"<assunto>",
"body":"<corpo>",
"recipients":["<destinatario_1>","<destinatario_2>",...,"<destinatario_N>"],
"html": <true_ou_false>
}
```

<b>Exemplo de Requisição</b>

Envio de mensagem do cliente de chave privada *8bb2c0e5dd2142c1957094b4670ebc2a* para dois destinatários:
```
POST /api/email HTTP/1.1
Host: localhost:5000
User-Agent: insomnia/2020.5.2
Authorization: ApiKey 8bb2c0e5dd2142c1957094b4670ebc2a
Content-Type: application/json
Accept: */*
Content-Length: 134

{
"subject":"Teste",
"body":"Isso é apenas um teste! :)",
"recipients":["paulo@email.com.br","barbara@email.com.br"],
"html": true
}
```

## Importante

Se você utiliza autenticação em duas etapas (2FA) em seu provedor de email será necessário uma *senha de aplicativo* para utilizar no *Email Sender*. Nesse caso, acesse sua conta de email e crie uma *senha de aplicativo* para poder enviar emails via API. No Gmail, por exemplo, esse procedimento pode ser realizado em https://myaccount.google.com/apppasswords.

<br/>
<hr>