# WhoIsFaster

**WhoIsFaster** é um jogo multiplayer onde dois jogadores competem para ver quem clica mais rápido. O jogo é jogado em salas, e os jogadores podem criar ou entrar em uma sala existente para competir entre si.

---

## Funcionalidades

### **Criação de Sala**
- O criador da sala pode definir o nome da sala e seu nome de usuário.
- **Regras:**
  - Não é possível criar uma sala com um nome que já exista.
  - O criador é adicionado como o primeiro jogador na sala.

### **Entrada em Sala**
- O jogador pode entrar em uma sala existente fornecendo o nome da sala e seu nome de usuário.
- **Regras:**
  - Não é possível entrar em uma sala com o mesmo nome de um jogador já presente na sala.
  - As salas têm um limite de 2 jogadores. Se a sala já estiver cheia, o jogador não poderá entrar.

### **Início do Jogo**
- O jogo começa assim que ambos os jogadores estiverem na sala.
- **Regras:**
  - O botão de iniciar jogo só estará disponível quando a sala tiver pelo menos 2 jogadores.

### **Jogo em Andamento**
- Quando o jogo começa, os jogadores competem clicando em um botão.
- Cada clique tem um tempo registrado, e o jogador com o menor tempo é o vencedor.

### **Notificação do Vencedor**
- Assim que ambos os jogadores clicarem, o vencedor é notificado automaticamente.
- O vencedor é o jogador com o menor tempo de resposta.

---

## Regras de Negócio

- **Limitação de Jogadores:**
  - Uma sala pode ter no máximo 2 jogadores. Se a sala já tiver 2 jogadores, não será possível adicionar mais.
  
- **Verificação de Nome de Usuário:**
  - Não é possível criar um nome de usuário duplicado dentro da mesma sala. Se um jogador já estiver usando o nome escolhido, será necessário escolher outro.

- **Conclusão do Jogo:**
  - O jogo termina quando ambos os jogadores clicarem. O vencedor é o jogador com o menor tempo de resposta.
  - Após a conclusão do jogo, a sala é removida e os jogadores são desconectados.

---

## Tecnologias Utilizadas

- **Frontend:**
  - Angular
  - SignalR

- **Backend:**
  - ASP.NET Core
  - SignalR

---

## Como Rodar o Projeto

### **Pré-requisitos:**
- .NET 8
- Node.js e npm

### **Passos para Executar o Backend:**
1. Clone o repositório.
2. Navegue até a pasta do backend.
3. Execute o comando:
   ```bash
   dotnet run
   ```

## Passos para Executar o Frontend:

1. Navegue até a pasta do frontend.
2. Execute o comando:
   ```bash
   npm install
3. Execute o comando para iniciar o servidor de desenvolvimento:
   ```bash
   npm run start

### **Atente-se nas variáveis de ambiente na pasta src/environments/environments.ts, caso o seu backend rodou em uma porta diferente no localhost troque-a no arquivo do frontend para funcionar.**

### **Considerações Finais**
O projeto foi desenvolvido como uma aplicação simples de jogo multiplayer com foco na interação em tempo real usando SignalR.
As funcionalidades podem ser expandidas para suportar mais jogadores, funcionalidades de chat, ou outras mecânicas de jogo.
