using Business.Models;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Business.Services;

public class CustomerService
{
    private readonly CustomerRepository _customerRepository;

    public CustomerService(CustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerEntity?> CreateCustomerAsync(CustomerRegistrationForm registrationForm)
    {
        var customerEntity = new CustomerEntity
        {
            CustomerName = registrationForm.CustomerName,
        };

        return await _customerRepository.CreateAsync(customerEntity); 
    }
     /* 
       I thought of not creating a CustomerModel, since it is representing 
       the exact same properties as the CustomerEntity. 
       But i did it to keep it separated for my own "learning/development", and learn more on mapping. 
       Normally, the CustomerModel wouldn't be created otherwise.
    */

    public async Task<CustomerEntity> CreateCustomerIfNotExistAsync(string customerName)
    {
        var existingCustomer = await _customerRepository.GetCustomerByNameAsync(customerName);
        if (existingCustomer != null)
        {
            return existingCustomer;
        }

        var customerEntity = new CustomerEntity { CustomerName = customerName };
        return await _customerRepository.CreateAsync(customerEntity);
    }

    public async Task<IEnumerable<CustomerModel>> GetCustomersAsync()
    {
        // This gets all the customers from the repository as CustomerEntity
        var customerEntities = await _customerRepository.GetAllAsync();
        /*
         I got some help from Chatgpt-4o with the LINQ here. 
         But what i am doing here is mapping each CustomerEntity to a CustomerModel. 
         Mapping Id and CustomerName to respective property in the Modell.
        */
        var customerModels = customerEntities.Select(c => new CustomerModel
        {
            Id = c.Id,
            CustomerName = c.CustomerName,
        });

        return customerModels;
    }

    public async Task<CustomerEntity?> GetCustomerByIdAsync(int customerId)
    {
        try
        {
            return await _customerRepository.GetCustomerByIdAsync(customerId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in GetCustomerByIdAsync: {ex}");
            return null;
        }
    }

    public async Task<bool> UpdateCustomerAsync(CustomerModel updatedCustomerModel)
    {
        var updatedCustomerEntity = new CustomerEntity
        {
            Id = updatedCustomerModel.Id,
            CustomerName = updatedCustomerModel.CustomerName,
        };
     
        return await _customerRepository.UpdateAsync(updatedCustomerEntity);
    }

    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        return await _customerRepository.DeleteAsync(customerId);
    }
}
