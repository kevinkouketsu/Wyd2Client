# Wyd2Client

Este é um simulador de cliente feito em C#, WPF usando um pouco do pattern MVVM.   

A ideia do projeto é poder implementar em qualquer outra aplicação facilmente. Foi criada através de eventos. Basta referenciar a Wyd2.Control em seu projeto próprio e utilizar dos benefícios dos eventos.

## Funções

Atualmente este cliente pode:   

* Logar em uma conta
* Criar personagens
* Deletar personagens
* Logar em personagem 
* Andar pelo jogo
* Usar teleportes 
* Ativar macro 
- Reconhecimento de chat normal / whisper
- Enviar mensagens no chat normal e whisper 

O macro está bem básico. E só funciona para personagens físicos. A ideia foi criar um dispatcher de macros e uma interface genérica para adicionar numa lista que vai iterar sobre todos os macros.
Sendo assim, teríamos um sistema de fácil inclusão de novos macros. Com isto, macros eventos, macros de buff, macros de qualquer situação seriam produzidos facilmente.

O sistema está preparado para logar no WYD Global (oficial RaidHut). 

Para teleportar, basta digitar no chat: #tele. 

## Imagens
Tela de login. Escolha um canal na lista, digite seu nome de usuário, senha e senha numérica.

![Tela de Login](https://github.com/kevinkouketsu/Wyd2Client/blob/master/docs/wyd2client-1.png)

Seleção de personagem. 
![Dentro do jogo](https://github.com/kevinkouketsu/Wyd2Client/blob/master/docs/wyd2client-4.png)

Dentro do jogo. 
![Dentro do jogo](https://github.com/kevinkouketsu/Wyd2Client/blob/master/docs/wyd2client-2.png)
![Dentro do jogo](https://github.com/kevinkouketsu/Wyd2Client/blob/master/docs/wyd2client-3.png)

### Bugs conhecidos

Temos alguns bugs corrigidos, simples de serem resolvidos:
- Não há sincronismo entre thread de envio e a interface. 
    - Com isto, as vezes, pode acontecer de uma lista estar sendo alterada enquanto outra está acessando e etc. O ideal seria melhorar a parte de Recv do cliente (Wyd2.Control)

### Agradecimento
Agradecimento a André Santa Cruz (ptr0x) de onde tirei a base da Wyd2.Common, que possui algumas informações básicas do WYD e algumas estruturas.

## Sobre

Projeto criado apenas para estudos. O mau uso do software fica por sua conta e risco. 
