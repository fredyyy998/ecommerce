package com.example.offersb.repositories;

import com.example.offersb.models.Product;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

public interface IProductRepository extends JpaRepository<Product, UUID> {
}
