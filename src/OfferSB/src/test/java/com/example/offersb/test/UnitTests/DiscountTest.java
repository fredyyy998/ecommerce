package com.example.offersb.test.UnitTests;

import com.example.offersb.models.Discount;
import org.junit.jupiter.api.Test;

import java.time.LocalDateTime;

public class DiscountTest {

    @Test
    public void testDiscount_Is_Instantiated_Correctly() {
        var dateStart = LocalDateTime.now();
        var dateEnd = LocalDateTime.MAX;
        var discount = Discount.create(10, dateStart, dateEnd);

        assert discount.getDiscountRate() == 10;
        assert discount.getStartDate() == dateStart;
        assert discount.getEndDate() == dateEnd;
    }
}
