using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Service.IService
{
	public interface ICartService
	{
		public event Action OnChange;
		Task DeCrementCart(ShoppingCart shoppingCart);

		Task InCrementCart(ShoppingCart shoppingCart);
		Task<int> GetQuantityInCart(int productId);
	}
}
