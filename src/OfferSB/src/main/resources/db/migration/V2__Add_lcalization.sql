ALTER TABLE offer ADD COLUMN localization_id varchar(255);

CREATE TABLE localization (
                              country_code varchar(255) NOT NULL,
                              country_name varchar(255) NOT NULL,
                              local_name varchar(255) NOT NULL,
                              currency varchar(255) NOT NULL,
                              PRIMARY KEY (country_code)
);

INSERT INTO localization (country_code, country_name, local_name, currency) VALUES
                                                                                ('DE', 'Germany', 'Deutschland', 'EUR'),
                                                                                ('GB', 'United Kingdom', 'United Kingdom', 'GBP'),
                                                                                ('US', 'United States', 'United States', 'USD'),
                                                                                ('CH', 'Switzerland', 'Switzerland', 'CHE');

UPDATE offer SET localization_id = (
    SELECT country_code FROM localization WHERE currency = offer.currency_code);

ALTER TABLE offer ADD CONSTRAINT fk_offer_localization FOREIGN KEY (localization_id) REFERENCES localization(country_code);

ALTER TABLE offer DROP COLUMN currency_code;