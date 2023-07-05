package com.example.offersb.models;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;


@Entity
@AllArgsConstructor
@NoArgsConstructor
public class Localization {

    @Id
    @Getter
    private String countryCode;

    @Getter
    private String countryName;

    @Getter
    private String localName;

    @Getter
    private String currency;

    public static Localization create(String countryCode, String countryName, String localName, String currency) {
        return new Localization(countryCode, countryName, localName, currency);
    }
}
