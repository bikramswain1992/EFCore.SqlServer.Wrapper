EFCore.SqlServer.Wrapper
=========================
Provides easy configuration of EFCore SQL server with dependancy injection. Give access to basic functionalities with inbuilt error handeling and formatted result.

Usage
---------
Library can be used to create single or multiple **DbContext** depending on the need. As a example we will consider to database entities **Country** and **State**. Below is the structure for these entities.

> public class Country : VersionedEntityBase  
{  
&nbsp;&nbsp;  public string Name {get; set;}  
&nbsp;&nbsp;  public string IsdCode {get; set;}  
//Navigation property
&nbsp;&nbsp;  public List<State> States {get; set;}
}  
   
> public class State : EntityBase  
{  
&nbsp;&nbsp;  public string Name {get; set;}  
&nbsp;&nbsp;  public int Population {get; set;}  
&nbsp;&nbsp;  public Guid CountryId {get; set;}  
//Navigation property
&nbsp;&nbsp;  public Country Country {get; set;}
}  

All entities have to be either of type **EntityBase** or **VersionedEntityBase**. **EntityBase** class provides **Id** property which is of **Guid** type and can be used as the primary key of the database object. Use **VersionedEntityBase** if along with **Id** you also require auditing properties like **CreatedBy**, **ModifiedBy**, **CreatedOn** and **ModifiedOn**.

Use **Fluent Api** to define your database structure. Use **IEntityBuilder** interface with EntityType created in previous step to access and configure **ModelBuilder**  
> public class CountryBuilder : IEntityBuilder<`Country`>  
  {  
  &nbsp;&nbsp;  public void BuildModel(ModelBuilder builder)  
  &nbsp;&nbsp;  {  
  &nbsp;&nbsp;&nbsp;&nbsp;    var entityBuilder = builder.Entity<Country>();  
  &nbsp;&nbsp;&nbsp;&nbsp;    entityBuilder.HasKey(x => x.Id);  
  &nbsp;&nbsp;  }  
  }
  
  > public class StateBuilder : IEntityBuilder<`Country`>  
  {  
  &nbsp;&nbsp;  public void BuildModel(ModelBuilder builder)  
  &nbsp;&nbsp;  {  
  &nbsp;&nbsp;&nbsp;&nbsp;  var entityBuilder = builder.Entity<State>();  
  &nbsp;&nbsp;&nbsp;&nbsp;  entityBuilder.HasKey(x => x.Id);  
  &nbsp;&nbsp;&nbsp;&nbsp;  entityBuilder.HasOne(x => x.Country).WithMany(x => x.States).HasForeignKey(x => x.CountryId);
  &nbsp;&nbsp;  }  
  }
  
After defining the enities and their relations configure the dependency injection and DbContext generation.  
> var builder = WebApplication.CreateBuilder(args);  
builder.service.AddTransient<IEntityBuilder<`Country`>, CountryBuilder>();  
builder.service.AddTransient<IEntityBuilder<`Country`>,StateBuilder>();  
  
> builder.service.CreateSqlContext<`Country`>(configuration);  
  
Here **configuration** in the above code is IConfiguration object which .net core provides. The EFCore.SQLServer.Wrapper package automatically takes the connection string from the **configuration** object. The name of the connection string in **appSettings.json** should be **DefaultConnection**. Alternative you can provide your own connection string name by following the below structure.
  
> builder.service.CreateSqlContext<`Country`>(configuration, `connectionstringname`);
  
If the above configurations are configured properly, .net automatically injects the **database entity** for **IEntityRepository<`Country`>** using constructor dependency injection. Basic operations has been demonstrated in below example.
  
> public class DatabaseOperations  
  {  
    private readonly IEntityRepository<`Country`> _repository;  
  
    public DatabaseOperations(IEntityRepository<`Country`> repository)  
    {  
      _repository = repository;  
    }  
  
    public List<Country> GetAllCountries()  
    {  
      var result = _repository.GetAll();
      return result;
    }  
    
    public Country GetCountryById(Guid countryId)  
    {  
      var result = _repository.Get(countryId);
      return result;
    }  
  
    public Country GetCountryWithStatesById(Guid countryId)  
    {  
      var result = _repository.AsQuerable().Include(x => x.States).FirstOrDefault(x => x.Id == countryId);  
      return result;  
    }  
  
    public Guid CreateCountry(Country country)  
    {  
      var result = _repository.Create(country);  
      return result;  
    }  
  
    public Guid UpdateCountry(Country country)  
    {  
      var result = _repository.AsQuerable().Include(x => x.States).FirstOrDefault(x => x.Id == countryId);  
      result.Name = country.Name;  
      result.States = country.States;  
      
      var updateResult = _repository.Update(result);  
      return updateResult;  
    }  
  
    public Guid DeleteCountry(Guid countryId)  
    {  
      var result = _repository.Delete(countryId);  
      return result;  
    }  
