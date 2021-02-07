using System.Linq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Services
{
    public class CartService : ICartService
    {
        private readonly IProductData _ProductData;
        private readonly ICartStore _cartStore;

        public CartService(IProductData productData, ICartStore cartStore)
        {
            _ProductData = productData;
            _cartStore = cartStore;
        }

        private Cart Cart
        {
            get => _cartStore.Cart;
            set => _cartStore.Cart = value;
        }

        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                cart.Items.Add(new CartItem {ProductId = id, Quantity = 1});
            else
                item.Quantity++;

            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if(item is null) return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity == 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null) return;

            cart.Items.Remove(item);

            Cart = cart;
        }

        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }

        public CartViewModel TransformFromCart()
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var product_view_models = products.ToView();

            return new CartViewModel
            {
                Items = Cart.Items.Join(product_view_models,
                    cartItem => cartItem.ProductId,
                    productVM => productVM.Id,
                    (cartItem, productVM) => (productVM, cartItem.Quantity)
                )
            };
        }
    }
}
