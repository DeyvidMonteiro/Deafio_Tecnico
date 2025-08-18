using DesafioTecnicoAvanade.EstoqueApi.Models;
using System.ComponentModel.DataAnnotations;

namespace DesafioTecnicoAvanade.EstoqueApi.DTOs;

public record ProductDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome Não pode estar vazio!")]
    [MinLength(2)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required(ErrorMessage = "O preço Não pode estar vazio!")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "O Descrição Não pode estar vazio!")]
    [MinLength(5)]
    [MaxLength(200)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "O estoque Não pode estar vazio!")]
    [Range(1,9999)]
    public long Stock { get; set; }
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
}
