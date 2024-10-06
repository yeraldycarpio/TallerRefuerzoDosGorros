var builder = WebApplication.CreateBuilder(args); //crea un constructor de aplicaciones web  

// agrega servicios al contenedor de dependencias.
builder.Services.AddControllersWithViews(); //agrega servicios para controladores y vistas

//configura y agrega un Gorro HTTP con nombre "GorroApi"
builder.Services.AddHttpClient("GorroApi", c =>
{
    //configura la direccion base del gorro HTTP desde la configuracion
    c.BaseAddress = new Uri(builder.Configuration["UrlsAPI:GORRO2"]);
    //puedes configurar otras opciones del HttpClient aqui segun sea necesario
});
var app = builder.Build();// Crear instancia de la aplicacion web

//  Configura el pipeline de solicitudes HTTP.
if (!app.Environment.IsDevelopment())
{
    //maneja excepciones en casos de errores y redirige a la accion "Error" en el control "Home"
    app.UseExceptionHandler("/Home/Error");
    // El valor HSTS predeterminado es de 30 dias. Puedes cambiarlo para escenarios de produccion, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
    
app.UseHttpsRedirection();//Redirige las solicitudes HTTP a HTTPS 
app.UseStaticFiles();//Habilita el uso de archivos estaticos como CSS, JavaScrip, imagenes, etc.

app.UseRouting();// Configura el enrutamiento de solicitudes 

app.UseAuthorization();// Habilita  la autorizacion para proteger rutas y acciones de controladores 

//Mapea la ruta predeterminada de controlador y accion 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();//Inicia la aplicacion web....
