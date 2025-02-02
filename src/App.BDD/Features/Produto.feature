Feature: Produto
Para verificar a funcionalidade de busca de produtos
Como um usuário
Eu quero buscar um produto pelo seu ID

@tag1
Scenario: [Buscar produto existente pelo ID]
	Given [que existe um produto com ID 1]
	When [eu buscar o produto pelo IDCategoria 1]
	Then o produto com ID 1 deve ser retornado
