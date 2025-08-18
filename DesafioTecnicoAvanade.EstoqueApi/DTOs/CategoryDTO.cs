using DesafioTecnicoAvanade.EstoqueApi.Models;
using System.ComponentModel.DataAnnotations;

namespace DesafioTecnicoAvanade.EstoqueApi.DTOs;

public record CategoryDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage ="O nome Não pode estar vazio!")]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Name { get; set; }
    public ICollection<Product>? Products { get; set; }

}
