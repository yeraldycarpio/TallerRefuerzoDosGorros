using AutoMapper;
using Gorros.DTOs;
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
            app.MapPost("/gorro/search", async (SearchQueryGorrosDTO gorroDTO, GorrosDAL gorrosDAL, IMapper mapper) =>
            {
                var gorro = new Gorro
                {
                    nombre = gorroDTO.Nombre_Like ?? string.Empty
                };

                var producters = await gorrosDAL.Search(gorro, skip: gorroDTO.Skip, take: gorroDTO.Take);
                var conutRow = 0;

                if (gorroDTO.SendRowCount == 2)
                {
                    conutRow = await gorrosDAL.CountSearch(gorro);
                }

                var productResult = new SearchResultGorrosDTO
                {
                    Data = mapper.Map<List<SearchResultGorrosDTO.GorroDTO>>(producters),
                    CountRow = conutRow
                };

                return productResult;
            });

            //Metodo para Obtener por ID
            app.MapGet("/gorro/{id}", async (int id, GorrosDAL gorrosDAL, IMapper mapper) =>
            {
                var gorro = await gorrosDAL.GetById(id);
                if (gorro == null || gorro.Id == 0)
                    return Results.NotFound(new GetIdResultGorrosDTO());

                var gorroResult = mapper.Map<GetIdResultGorrosDTO>(gorro);
                return Results.Ok(gorroResult);
            });

            // Crear nuevo gorro
            app.MapPost("/gorro", async (CreateGorrosDTO create, GorrosDAL gorrosDAL, IMapper mapper) =>
            {
                var gorro = mapper.Map<Gorro>(create);
                int result = await gorrosDAL.Create(gorro);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Editar
            app.MapPut("/gorro", async (EditGorrosDTO edit, GorrosDAL gorrosDAL, IMapper mapper) =>
            {
                var gorro = mapper.Map<Gorro>(edit);
                int result = await gorrosDAL.Edit(gorro);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Eliminar
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
