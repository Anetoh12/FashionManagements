using System;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using FashionManagement.Cart;
using FashionManagement.Models;
namespace FashionManagement.Services
{
    public class CartService
    {
        private readonly ProtectedLocalStorage _storage;
        private bool _loaded;
        private readonly List<CartItem> _items = new();

       public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
        public int TotalItems => _items.Sum(i => i.Quantity);
        public decimal TotalPrice => _items.Sum(item => item.SubTotal);
        public void AddToCart(Product product)
        {
            if (product.StockQuantity <= 0)
            {
                throw new InvalidOperationException("This product is out of stock.");
            }
            var existingItem = _items.FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem != null)
            {
                if (existingItem.Quantity >= product.StockQuantity)
                {
                    throw new InvalidOperationException("Cannot add more items than available in stock.");
                }
                existingItem.Quantity++;
            }
            else
            {
                _items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            Save();
        }
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                return;
            }
            if (quantity <= 0)
            {
                RemoveFromCart(productId);
                return;
            }
            item.Quantity = quantity;
        }

        public void RemoveFromCart(int productId)
        {
            var item = _items.FirstOrDefault(i =>  i.ProductId == productId);

            if (item != null)
            {
                _items.Remove(item);
            }
        }
        public void ClearCart()
        {
            _items.Clear();
        }
        public CartService(ProtectedLocalStorage storage)
        {
            _storage = storage;
        }
        public async Task InitializeAsync()
        {
            if (_loaded) return;
            _loaded = true;
            var result = await _storage.GetAsync<List<CartItem>>("cart");
            
            if (result.Success && result.Value != null)
            {
                _items.Clear();
                _items.AddRange(result.Value);
            }
        }
        private void Save() => _ = _storage.SetAsync("cart", _items);
    }
}
