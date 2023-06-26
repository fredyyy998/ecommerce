create table offer (discount_rate float(53) not null, gross_price float(53) not null, net_price float(53) not null, tax_rate float(53) not null, discount_end_date datetime(6), discount_start_date datetime(6), end_date datetime(6), start_date datetime(6), id binary(16) not null, product_id binary(16), dtype varchar(31) not null, currency_code varchar(255), name varchar(255), primary key (id)) engine=InnoDB;
create table offer_products (package_offer_id binary(16) not null, products_id binary(16) not null) engine=InnoDB;
create table product (id binary(16) not null, description varchar(255), name varchar(255), primary key (id)) engine=InnoDB;
alter table offer add constraint FK3cow2cmfxb0nrt43hxm7yu1q3 foreign key (product_id) references product (id);
alter table offer_products add constraint FK1yt4s1krrnlserh80d6lvj28p foreign key (products_id) references product (id);
alter table offer_products add constraint FKnbwio4ndfmbmj0lsqwl9pd4gg foreign key (package_offer_id) references offer (id);
