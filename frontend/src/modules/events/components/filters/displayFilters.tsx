import React from 'react';
import { Box, Button } from '@mui/material';
import PlaceFilter from './placeFilter';
import CategoryFilter from './categoryFilter';
import TimeFilter from './timeFilter';
import useSetPlaceFilter from '../../store/hooks/filters/useSetPlaceFilter';
import useSetCategoryFilter from '../../store/hooks/filters/useSetCategoryFilter';
import useSetTimeFilter from '../../store/hooks/filters/useSetTimeFilter';

const DisplayFilters = () => {
    const setPlaceFilter = useSetPlaceFilter();
    const setCategoryFilter = useSetCategoryFilter();
    const setTimeFilter = useSetTimeFilter();

    const handleClear = () => {
        setPlaceFilter('');
        setCategoryFilter('');
        setTimeFilter(undefined);
    }
    
    return (
        <Box sx={{ p: 3, mt: 4 }}>
            <PlaceFilter />
            <CategoryFilter />
            <TimeFilter />
            <Button onClick={handleClear} sx={{ ml: 2 }}>Clear Filters</Button>
        </Box>
    );
};

export default DisplayFilters;
