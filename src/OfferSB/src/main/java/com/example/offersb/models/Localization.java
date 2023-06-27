package com.example.offersb.models;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import lombok.Getter;



@Entity
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

}
