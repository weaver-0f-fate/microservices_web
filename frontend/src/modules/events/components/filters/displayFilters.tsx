import React from 'react';
import { Box, Button } from '@mui/material';
import PlaceFilter from './placeFilter';
import CategoryFilter from './categoryFilter';
import TimeFilter from './timeFilter';
import { setCategoryFilter, setPlaceFilter, setTimeFilter } from "../../store/filtersSlice";
import { useDispatch } from "react-redux";

const DisplayFilters = () => {
    const dispatch = useDispatch();

    const handleClear = () => {
        dispatch(setPlaceFilter(''));
        dispatch(setCategoryFilter(''));
        dispatch(setTimeFilter(undefined));
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
