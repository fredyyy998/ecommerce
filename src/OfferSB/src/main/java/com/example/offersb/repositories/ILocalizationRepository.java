package com.example.offersb.repositories;

import com.example.offersb.models.Localization;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ILocalizationRepository extends JpaRepository<Localization, String> {
}
