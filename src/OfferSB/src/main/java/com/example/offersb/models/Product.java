package com.example.offersb.models;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;

import java.util.UUID;

@NoArgsConstructor
public class Product
{
    @Getter
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
