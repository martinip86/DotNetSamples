const { ApolloServer, gql } = require('apollo-server');
const axios = require('axios').default;
const https = require('https');

const typeDefs = gql`
  type WeatherForecast {
    date: String
    temperatureC: Float
    summary: String
  }
 
  type Query {
    weather: [WeatherForecast]
  }
`;

const resolvers = {
  Query: {
    weather: async () => {
      const { data } = await axios.get("https://localhost:32798/weatherforecast",  { httpsAgent: new https.Agent({  
        rejectUnauthorized: false
      }) });
      
      return data;      
    },
  },
};
 
const server = new ApolloServer({ typeDefs, resolvers });

server.listen().then(({ url }) => {
  console.log(`🚀  Server ready at ${url}`);
});