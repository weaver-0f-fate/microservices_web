import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import EventsCalendar from '../../modules/events/eventsCalendar';
import Layout from '../layout/layout';
import Algorithms from '../../modules/algorithms/algorithms';

const AppRouter = () => {
  return (
    <Layout>
      <Router>
        <Routes>
          <Route path="/" element={<EventsCalendar /> } />
          <Route path="/events" element={<EventsCalendar /> } />
          <Route path="/algorithms" element={<Algorithms />} />
        </Routes>
      </Router>
    </Layout>
  );
};

export default AppRouter;
