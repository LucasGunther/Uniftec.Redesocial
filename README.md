# Uniftec.Redesocial

Projeto ASP.NET MVC para simulação de uma rede social, com funcionalidades como:

- Busca de usuários
- Integração com APIs externas de colegas (mock)

---

## Novidades recentes

### Integração com API externa

- Agora o sistema consome dados de **usuários externos simulados** via `HttpClient`.
- Endpoint da API externa: `http://localhost:5136/api/UsuariosFake`
- Os dados retornados são exibidos na view `Buscar.cshtml` com uma **tag indicativa `Externo`**.

## Implementações

- Método `UsuariosColega` no `UsuarioController`
- Consumo de API via `HttpClient` injetado
- Exibição reaproveitando layout de busca local
- Dados recebidos diretamente em `UsuarioModel`

---

## Como testar

1. Rode a API `Uniftec.Redesocial.MockAPI` (porta padrão: `5136`)
2. Acesse o projeto principal em:  
http://localhost:PORTA/Usuario/UsuariosColega

3. Os usuários da API externa aparecerão com visual idêntico aos locais, marcados com:
   ```
   <span class="badge badge-secondary ml-2">Externo</span>
   ```

