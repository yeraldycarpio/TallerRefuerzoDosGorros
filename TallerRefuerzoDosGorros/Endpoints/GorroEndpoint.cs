using Gorros.DTOs.GorrosDTOS;
using TallerRefuerzoDosGorros.Models.DAL;
using TallerRefuerzoDosGorros.Models.EN;

namespace TallerRefuerzoDosGorros.Endpoints
{
    public static class GorroEndpoint
    {
        public static void AddGorroEndpoints(this WebApplication app)
        {
            //Metodo para Buscar
            app.MapPost("/gorro/search", async (SearchQueryGorrosDTO gorroDTO, GorrosDAL gorrosDAL) =>
            {
                var gorro = new Gorro
                {
                    nombre = gorroDTO.Nombre_Like != null ? gorroDTO.Nombre_Like : string.Empty
                };

                var producters = new List<Gorro>();
                int conutRow = 0;

                if (gorroDTO.SendRowCount == 2)
                {
                    producters = await gorrosDAL.Search(gorro, skip: gorroDTO.Skip, take: gorroDTO.Take);
                    if (producters.Count > 0)
                        conutRow = await gorrosDAL.CountSearch(gorro);
                }
                else
                {
                    producters = await gorrosDAL.Search(gorro, skip: gorroDTO.Skip, take: gorroDTO.Take);
                }
                var productResult = new SearchResultGorrosDTO
                {
                    Data = new List<SearchResultGorrosDTO.GorroDTO>(),
                    CountRow = conutRow
                };
                producters.ForEach(a =>
                {
                    productResult.Data.Add(new SearchResultGorrosDTO.GorroDTO
                    {
                        Id = a.Id,
                        Nombre = a.nombre,
                        Color = a.color,
                        Material = a.material,
                        Precio = a.precio
                    });
                });
                return productResult;
            });


            //Metodo para Obtener por ID
            /*
            app.MapGet("/gorro/{id}", async (int id, GorrosDAL gorrosDAL) =>
            {
                var gorros = await gorrosDAL.GetById(id);

                var gorroResult = new GetIdResultGorrosDTO
                {
                    Id = gorros.Id,
                    Nombre = gorros.nombre,
                    Color = gorros.color,
                    Material = gorros.material,
                    Precio = (decimal)gorros.precio
                };

                if (gorroResult.Id > 0)
                    return Results.Ok(gorroResult);
                else
                    return Results.NotFound(gorroResult);
            });*/

            //Metodo para Obtener por ID
            app.MapGet("/gorro/{id}", async (int id, GorrosDAL gorrosDAL) =>
            {
                var gorro = await gorrosDAL.GetById(id);
                if (gorro == null || gorro.Id == 0)
                    return Results.NotFound(new GetIdResultGorrosDTO());
                var gorroResult = new GetIdResultGorrosDTO
                {
                    Id = gorro.Id,
                    Nombre = gorro.nombre,
                    Color = gorro.color,
                    Material = gorro.material,
                    Precio = (decimal)gorro.precio
                };

                if (gorroResult.Id > 0)
                    return Results.Ok(gorroResult);
                else
                    return Results.NotFound(gorroResult);
            });

            //Crear nuevo gorro
            app.MapPost("/gorro", async (CreateGorrosDTO create, GorrosDAL gorrosDAL) =>
            {
                var gorros = new Gorro
                {
                    nombre = create.Nombre,
                    color = create.Color,
                    material = create.Material,
                    precio = create.Precio
                };

                int result = await gorrosDAL.Create(gorros);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            //Editar
            app.MapPut("/gorro", async (EditGorrosDTO edit, GorrosDAL gorrosDAL) =>
            {
                var gorros = new Gorro
                {
                    Id = edit.Id,
                    nombre = edit.Nombre,
                    color = edit.Color,
                    material = edit.Material,
                    precio = edit.Precio
                };

                int result = await gorrosDAL.Edit(gorros);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            //Eliminar
            app.MapDelete("/gorro/{id}", async (int id, GorrosDAL delete) =>
            {
                int result = await delete.Delete(id);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });
        }
    }
}
