# Integração de Sistemas – Tarefa 5 - Atividade II (gRPC)

Este repositório contém a implementação de um middleware gRPC e duas aplicações cliente autónomas,
no contexto da Unidade Curricular Integração de Sistemas (MEIW – Universidade Aberta).

# Objetivo
Implementar e testar serviços gRPC para duas entidades independentes:
- **Entidade de Registo**
- **Entidade de Votação**

Os serviços foram inicialmente testados com a ferramenta *grpcurl* e, posteriormente,
foram desenvolvidas duas aplicações cliente gRPC em .NET.

# Estrutura do Repositório

│
├── GrpcService # Servidor gRPC (ASP.NET Core)
│ ├── Protos # Ficheiros .proto (registro, votacao)
│ ├── Services # Implementações dos serviços gRPC
│ └── Program.cs
│
├── Client.Registro # Cliente gRPC – Entidade de Registo
├── Client.Votacao # Cliente gRPC – Entidade de Votação
│

# Tecnologias Utilizadas
- ASP.NET Core gRPC
- Protocol Buffers (.proto)
- grpcurl
- .NET 10 (Console Applications)
- Visual Studio Community 2026

# Pré-requisitos
- .NET SDK 8 ou superior
- grpcurl (https://github.com/fullstorydev/grpcurl)
- Windows (CMD ou PowerShell)


# Como Executar

# 1) Executar o Servidor gRPC
```cmd
cd GrpcService
dotnet run
```
O servidor inicia, por defeito, em:

HTTP: http://127.0.0.1:5038
HTTPS: https://localhost:7055

```cmd
C:\Tools\grpcurl\grpcurl.exe -plaintext 127.0.0.1:5038 list
```

Criar um request.json e inserir (Exemplo):
{
  "name": "Assis",
  "document": "123456789"
}


Depois correr no terminal:
```cmd
C:\Tools\grpcurl\grpcurl.exe -plaintext -d @ 127.0.0.1:5038 registro.RegistroService/CreateVoter < request.json
```

# 2) Executar Cliente – Registo
```cmd
cd Client.Registro
set GRPC_SERVER=http://127.0.0.1:5038
dotnet run
```

# 3) Executar Cliente – Votacao
```cmd
cd Client.Votacao
set GRPC_SERVER=http://127.0.0.1:5038
dotnet run
```

@author Assis Caetano
