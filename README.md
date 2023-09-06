# dotNet5783_9173_6297

Bonuses that we did


stage 1 and stage 2- Tryparse
					ToString


Stage 3- Extension methods for checking validation.
		 An option to delete a product which is not in any cart.

stage 4-

Lazy- Makes sure that the instance should not be allocated immediately when the function that returns it is called,
but just when its values are used. This is made to avoid a situation that a variable is initialized but never used,
and wastes resources unnecessarily.

Threadsafe- To avoid a situation that two threads will access an instance together and then it will be allocated twice.
When accessing an instance we lock it until we finish with it. By doing so, if two threads try accessing an instance together 
the first one will enter and lock the instance, the second one will wait until the first one will finish and only when the 
first one finished with the instance and releases it, the second one will have access to there, then it'll see that the 
instance is initialized and will use it.

Giving an option of 'all categories' in the category combobox for displaying products by category.


Stage 5- Enabling the administrator to change an amount, delete or add products to an existing order.
		 Binding- almost complete usage
		 Style resource
		 Converter
		 PO for the cart

Stage 7- Progress Bar
