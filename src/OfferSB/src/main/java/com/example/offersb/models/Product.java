package com.example.offersb.models;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import lombok.Getter;
import lombok.NoArgsConstructor;

import java.util.UUID;

@Entity
@NoArgsConstructor
public class Product
{
    @Getter
    @Id
    private UUID id;

    @Getter
    private String name;

    @Getter
    private String description;

    protected Product(UUID id, String name, String description)
    {
        this.id = id;
        this.name = name;
        this.description = description;
    }

    public static Product create(UUID id, String name, String description)
    {
        return new Product(id, name, description);
    }
}
